using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Data;
using System.Data.SqlClient;
using VulcanAnalytics.DBTester.Exceptions;

namespace VulcanAnalytics.DBTester
{
    public class MsSqlDatabaseTester : DatabaseTester
    {
        private SqlConnection connection = new SqlConnection();

        private Database database;

        public MsSqlDatabaseTester(string connectionString) : base()
        {
            defaultSchema = "dbo";
            Server dbServer;

            try
            {
                var builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = connectionString;

                connection.ConnectionString = builder.ConnectionString;

                ServerConnection conn = new ServerConnection(connection);

                dbServer = new Server(conn);
                var c = dbServer.Databases.Count;
            }
            catch (Exception e)
            {
                var newerror = new FailedDatabaseConnection("There was a problem with your connection string", e);
                throw newerror;
            }

            var connectionDatabase = connection.Database;

            this.database = dbServer.Databases[connectionDatabase];
        }

        public override bool HasSchema(string schemaName)
        {
            return this.database.Schemas.Contains(schemaName);
        }

        public override bool HasTable(string schemaName, string tableName)
        {
            return this.database.Tables.Contains(tableName, schemaName);
        }

        public override bool HasTable(string tableName)
        {
            return HasTable(base.defaultSchema, tableName);
        }

        public override int RowCount(string schemaName, string objectName)
        {
            var sqlStatement = string.Format("select count(*) from {0}.{1};", schemaName, objectName);

            var results = database.ExecuteWithResults(sqlStatement);

            var count = int.Parse(results.Tables[0].Rows[0][0].ToString());

            return count;
        }

        public override void ExecuteStatementWithoutResult(string sqlStatement)
        {
            database.ExecuteNonQuery(sqlStatement);
        }

        public override DataSet ExecuteStatementWithResult(string sqlStatement)
        {
            return database.ExecuteWithResults(sqlStatement);
        }
    }
}
