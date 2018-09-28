﻿using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Data.SqlClient;
using VulcanAnalytics.DBTester.Exceptions;

namespace VulcanAnalytics.DBTester
{
    public class DatabaseTester
    {
        private string defaultSchema;

        private SqlConnection connection = new SqlConnection();

        private Database database;

        public DatabaseTester(string connectionString)
        {
            defaultSchema = "dbo";
            Server dbServer;

            try
            {
                // Reset timeout to 2 seconds for faster test results
                var builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = connectionString;
                builder.ConnectTimeout = 2;

                connection.ConnectionString = builder.ConnectionString;

                ServerConnection conn = new ServerConnection(connection);

                dbServer = new Server(conn);
                var c = dbServer.Databases.Count;
            }
            catch (Exception e)
            {
                var newerror = new DatabaseTesterConnectionException("There was a problem with your connection string", e);
                throw newerror;
            }

            var connectionDatabase = connection.Database;

            this.database = dbServer.Databases[connectionDatabase];
        }


        public bool HasSchema(string schemaName)
        {
            return database.Schemas.Contains(schemaName);
        }

        public bool HasTable(string tableName)
        {
            return HasTable(tableName, defaultSchema);
        }

        public bool HasTable(string schemaName, string tableName)
        {
            return database.Tables.Contains(tableName, schemaName);
        }

        public int RowCount(string schemaName, string objectName)
        {
            var sqlStatement = string.Format("select count(*) from {0}.{1};",schemaName,objectName);

            var results = database.ExecuteWithResults(sqlStatement);

            var count = int.Parse(results.Tables[0].Rows[0][0].ToString());

            return count;
        }

        public void ExecuteStatementWithoutResult(string sqlStatement)
        {
            database.ExecuteNonQuery(sqlStatement);
        }

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
