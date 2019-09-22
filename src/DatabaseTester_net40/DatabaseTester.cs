using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

        public abstract string QuotedIdentifier(string identifier);

        public abstract string UnquotedIdentifier(string identifier);


        public void InsertData(string schemaName, string objectName, string[] columns, Object[] data)
        {
            if (columns.Length == 0)
            {
                throw new NoColumnsToInsert();
            }
            if (data.Length == 0)
            {
                throw new NoRowsToInsert();
            }

            string sqlColumns = SqlColumns(columns);

            foreach (Object[] row in data)
            {
                InsertRow(schemaName, objectName, sqlColumns, row);
            }
        }

        public void InsertData(string schemaName, string objectName, string[] columns, Object[] data, ColumnDefaults defaults)
        {
            if (data.Length == 0)
            {
                throw new NoRowsToInsert();
            }

            var columnsWithDefaultsAdded = ColumnsWithDefaultsAdded(columns,defaults);

            var sqlColumns = SqlColumns(columnsWithDefaultsAdded);

            foreach (Object[] row in data)
            {
                var newRow = CombineRowDataWithDefaults(row, columnsWithDefaultsAdded);

                InsertRow(schemaName, objectName, sqlColumns, newRow);
            }
        }

        public DataTable ObjectData(string schemaName, string tableName)
        {
            if (!HasTable(schemaName,tableName))
            {
                var message = string.Format("Couldn't find the object {0}.{1} in the database.", schemaName, tableName);
                throw new ObjectNotFound(message);
            }

            var sqlStatement = string.Format("select * from {0}.{1};",schemaName,tableName);

            var results = this.ExecuteStatementWithResult(sqlStatement);

            var table = results.Tables[0];

            return table;
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
                var cell = row[i];

                if (cell == null)
                {
                    cleanRow[i] = "null";
                }
                else
                {
                    string textData;

                    textData = ConvertCellToText(cell);

                    cleanRow[i] = string.Format("'{0}'", textData);
                }
                i++;
            }
            return cleanRow;
        }

        protected virtual string ConvertCellToText(object cell)
        {
            string textData;
            switch (cell.GetType().ToString())
            {
                case "System.DateTime":
                    textData = ((DateTime)cell).ToString("yyyy-MM-dd HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo);
                    break;

                default:
                    textData = cell.ToString().Replace("'", "''");
                    break;
            }

            return textData;
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
            object[] quotedColumns = QuotedColumns(columns);

            string sqlColumns = ArrayAsTemplatedString(quotedColumns, "{0}", ",");

            return sqlColumns;
        }

        private object[] QuotedColumns(object[] columns)
        {
            var quotedColumns = new object[columns.Length];

            var n = 0;
            while (n < columns.Length)
            {
                quotedColumns[n] = QuotedIdentifier(columns[n].ToString());
                n++;
            }

            return quotedColumns;
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

        private string SqlInsertStatement
            (
            string schemaName,
            string objectName,
            string sqlColumns,
            string sqlValues
            )
        {
            string template = "insert into {0}.{1}({2}) values({3});";

            var statement = string.Format
                (
                template,
                QuotedIdentifier(schemaName),
                QuotedIdentifier(objectName),
                sqlColumns,
                sqlValues
                );

            return statement;
        }

        #endregion
    }
}
