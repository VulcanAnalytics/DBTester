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
                var c = dbServer.Databases.Count; //ensure the connection/database is valid
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

        public override void ClearTable(string schemaName, string tableName)
        {
                try
                {
                    DeleteOrTruncateTable(schemaName, tableName);
                }
                catch (Exception e)
                {
                    throw new ChildTablesWithDataReferenceThisTable("Failed to clear table, is there another table with data referencing this table?", e);
                }
        }

        private void DeleteOrTruncateTable(string schemaName, string tableName)
        {
            try
            {
                TruncateTable(schemaName,tableName);
            }
            catch
            {
                DeleteTable(schemaName, tableName);
            }
        }

        private void TruncateTable(string schemaName, string tableName)
        {
            this.database.Tables[tableName, schemaName].TruncateData();
        }

        private void DeleteTable(string schemaName, string tableName)
        {
            var deleteStatement = string.Format("delete from [{0}].[{1}]", schemaName, tableName);
            ExecuteStatementWithoutResult(deleteStatement);
        }

        private bool IsReferencedByForeignKeys(string schemaName, string tableName)
        {
            var foreignKeyCheckStatement = string.Format("exec sp_fkeys @pktable_owner = '{0}', @pktable_name = '{1}';", schemaName, tableName);

            var results = ExecuteStatementWithResult(foreignKeyCheckStatement);

            if (results.Tables[0].Rows.Count == 0)
                return false;
            else
                return true;
        }

        public override void DropTable(string schemaName, string tableName)
        {
            if (this.database.Tables.Contains(tableName, schemaName))
            if (!IsReferencedByForeignKeys(schemaName, tableName))
                    this.database.Tables[tableName, schemaName].Drop();
            else
                throw new ChildTablesReferenceThisTable("Failed to drop table, there another table with foreign keys referencing this table.");
        }

        public override bool HasTable(string tableName)
        {
            return HasTable(base.defaultSchema, tableName);
        }

        public override int RowCount(string schemaName, string objectName)
        {
            var sqlStatement = string.Format("select count(*) from [{0}].[{1}];", schemaName, objectName);

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
            var results = database.ExecuteWithResults(sqlStatement);
            if (results.Tables.Count == 0)
            {
                var errorMessage = string.Format("The following statement didn't return any tables: {0}", sqlStatement);
                throw new StatementReturnedNoTables(errorMessage);
            }
            return results;
        }
    }
}
