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
            string sqlTemplate = "insert into {0}.{1}({2}) values({3});";

            string sqlColumns = string.Empty;
            foreach (var s in columns)
            {
                sqlColumns += string.Format("{0},",s);
            }
            sqlColumns = sqlColumns.TrimEnd(',');

            foreach (Object[] row in data)
            {
                string sqlValues = string.Empty;
                foreach (var s in row)
                {
                    sqlValues += string.Format("'{0}',", s);
                }
                sqlValues = sqlValues.TrimEnd(',');

                var sql = string.Format(sqlTemplate, schemaName, objectName, sqlColumns, sqlValues);

                try
                {
                    this.ExecuteStatementWithoutResult(sql);
                }
                catch (Exception e)
                {
                    throw new Exception(String.Format("Error encountered executing the insert statement: {0}",sql), e);
                }
            }
        }
    }
}
