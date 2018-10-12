using System;
using System.Collections.Generic;
using System.Data;
using VulcanAnalytics.DBTester.Exceptions;

namespace VulcanAnalytics.DBTester
{
    public abstract class DatabaseTester
    {
        protected string defaultSchema;

        public abstract bool HasSchema(string schemaName);

        public abstract bool HasTable(string tableName);

        public abstract bool HasTable(string schemaName, string tableName);

        public abstract void ClearTable(string schemaName, string tableName);

        public abstract void DropTable(string schemaName, string tableName);

        public abstract int RowCount(string schemaName, string objectName);

        public abstract void ExecuteStatementWithoutResult(string sqlStatement);

        public abstract DataSet ExecuteStatementWithResult(string sqlStatement);


        public void InsertData(string schemaName, string objectName, string[] columns, Object[] data)
        {
            if (columns.Length == 0)
            {
                throw new NoColumnsToInsert();
            }

            string sqlColumns = SqlColumns(columns);

            foreach (Object[] row in data)
            {
                InsertRow(schemaName, objectName, sqlColumns, row);
            }
        }

        public void InsertData(string schemaName, string objectName, string[] columns, Object[] data, ColumnDefaults defaults)
        {
            var columnsWithDefaultsAdded = ColumnsWithDefaultsAdded(columns,defaults);

            var sqlColumns = SqlColumns(columnsWithDefaultsAdded);

            foreach (Object[] row in data)
            {
                var newRow = CombineRowDataWithDefaults(row, columnsWithDefaultsAdded);

                InsertRow(schemaName, objectName, sqlColumns, newRow);
            }
        }


        #region Private Methods

        private void InsertRow(string schemaName, string objectName, string sqlColumns, object[] row)
        {
            var cleanRow = CleanAndQuoteColumns(row);

            string sqlValues = SqlValues(cleanRow);

            var sql = SqlInsertStatement(schemaName, objectName, sqlColumns, sqlValues);

            TryToInsertRow(sql);
        }

        private object[] CleanAndQuoteColumns (object[] row)
        {
            var cleanRow = new object[row.Length];
            var i = 0;
            while (i < row.Length)
            {
                if (row[i] == null)
                {
                    cleanRow[i] = "null";
                }
                else
                {
                    cleanRow[i] = string.Format("'{0}'",row[i].ToString().Replace("'", "''"));
                }
                i++;
            }
            return cleanRow;
        }

        private void TryToInsertRow(string insertStatement)
        {
            try
            {
                this.ExecuteStatementWithoutResult(insertStatement);
            }
            catch (Exception exception)
            {
                throw new Exception(String.Format("Error encountered executing the insert statement: {0}", insertStatement), exception);
            }
        }

        private string SqlColumns(Dictionary<string, object> columns)
        {
            var newColumns = new string[columns.Count];
            columns.Keys.CopyTo(newColumns, 0);
            var sqlColumns = SqlColumns(newColumns);
            return sqlColumns;
        }

        private string SqlColumns(Object[] columns)
        {
            string sqlColumns = ArrayAsTemplatedString(columns, "{0}", ",");

            return sqlColumns;
        }

        private Dictionary<string, object> ColumnsWithDefaultsAdded(Object[] columns, ColumnDefaults defaults)
        {
            var combinedColumns = new Dictionary<string, object>();
            if (columns != null) {
                foreach (string c in columns)
                {
                    combinedColumns.Add(c, "ColumnInData");
                }
            }
            foreach (KeyValuePair<string, object> d in defaults)
            {
                if(combinedColumns.ContainsKey(d.Key))
                {
                    combinedColumns[d.Key] = d.Value;
                }
                else
                {
                combinedColumns.Add(d.Key, d.Value);
                }
            }

            if (combinedColumns.Count == 0)
            {
                throw new NoColumnsToInsert();
            }

            return combinedColumns;
        }

        private object[] CombineRowDataWithDefaults(object[] row, Dictionary<string, object> columnsWithDefaultsAdded)
        {
            var newRow = new object[columnsWithDefaultsAdded.Count];
            var defaults = new object[columnsWithDefaultsAdded.Count];
            columnsWithDefaultsAdded.Values.CopyTo(defaults, 0);
            var i = 0;
            while (i < columnsWithDefaultsAdded.Count)
            {
                object value = null;
                if (row != null)
                {
                    if (row.Length > i)
                    {
                        value = row[i];
                    }
                    else
                    {
                        value = defaults[i];
                    }
                }
                else
                {
                    value = defaults[i];
                }

                newRow[i] = value;

                i++;
            }

            return newRow;
        }

        private string SqlValues(Object[] values)
        {
            string sqlValues = ArrayAsTemplatedString(values, "{0}",",");

            return sqlValues;
        }

        private string ArrayAsTemplatedString(Object[] array, string template, string seperator)
        {
            string arrayString = string.Empty;

            foreach (var arrayItem in array)
            {
                arrayString += string.Format(template, arrayItem) + seperator;
            }

            arrayString = arrayString.TrimEnd(seperator.ToCharArray());

            return arrayString;
        }

        private string SqlInsertStatement(string schemaName, string objectName, string sqlColumns, string sqlValues)
        {
            string template = "insert into {0}.{1}({2}) values({3});";

            var statement = string.Format(template, schemaName, objectName, sqlColumns, sqlValues);

            return statement;
        }

        #endregion
    }
}
