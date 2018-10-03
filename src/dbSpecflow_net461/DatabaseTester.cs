using System;
using System.Data;

namespace VulcanAnalytics.DBTester
{
    public abstract class DatabaseTester
    {
        protected string defaultSchema;

        public abstract bool HasSchema(string schemaName);

        public abstract bool HasTable(string tableName);

        public abstract bool HasTable(string schemaName, string tableName);

        public abstract int RowCount(string schemaName, string objectName);

        public abstract void ExecuteStatementWithoutResult(string sqlStatement);

        public abstract DataSet ExecuteStatementWithResult(string sqlStatement);

        public void InsertData(string schemaName, string objectName, string[] columns, Object[] data)
        {
            string sqlColumns = SqlColumns(columns);

            foreach (Object[] row in data)
            {
                string sqlValues = SqlValues(row);

                var sql = SqlInsertStatement(schemaName, objectName, sqlColumns, sqlValues);

                TryToInsertRow(sql);
            }
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

        private string SqlColumns(Object[] columns)
        {
            string sqlColumns = ArrayAsTemplatedString(columns, "{0}", ",");

            return sqlColumns;
        }

        private string SqlValues(Object[] values)
        {
            string sqlValues = ArrayAsTemplatedString(values, "'{0}'",",");

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
    }
}
