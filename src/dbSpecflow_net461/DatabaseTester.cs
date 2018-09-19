using Microsoft.SqlServer.Management.Common;
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
    }
}
