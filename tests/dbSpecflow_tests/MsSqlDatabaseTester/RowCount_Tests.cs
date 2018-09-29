using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests.MsSqlDatabaseTester
{
    [TestClass]
    public class RowCount_Tests
    {
        private const string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        private DatabaseTester tester = new VulcanAnalytics.DBTester.MsSqlDatabaseTester(connection);

        private const string schemaName = "dbo";
        private const string tableName = "testtable";

        [TestMethod]
        public void RowCountReturnsNumberOfRowsFromTable()
        {
            var expectedCount = 5;
            DropAndCreateTable(schemaName, tableName);
            var i = 0;
            while (i < expectedCount)
            {
                tester.ExecuteStatementWithoutResult("insert into [dbo].[testtable]([col1]) values(99);");
                i++;
            }

            var actualCount = tester.RowCount(schemaName, tableName);

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void RowCountReturnsNumberOfRowsFromView()
        {
            var expectedCount = 5;
            var viewName = "testview";
            DropAndCreateTable(schemaName, tableName);
            var i = 0;
            while (i < expectedCount)
            {
                tester.ExecuteStatementWithoutResult("insert into [dbo].[testtable]([col1]) values(99);");
                i++;
            }
            DropView(schemaName, viewName);
            tester.ExecuteStatementWithoutResult("create view [dbo].[testview] as select [col1] from [dbo].[testtable];");

            var actualCount = tester.RowCount(schemaName, viewName);

            Assert.AreEqual(expectedCount, actualCount);
        }

        #region Private Methods

        private void DropAndCreateTable(string schemaName, string tableName)
        {
            DropTable(schemaName, tableName);
            CreateTable(schemaName, tableName);
        }

        private void DropTable(string schemaName, string tableName)
        {
            var sql = DropTableSql(schemaName, tableName);

            tester.ExecuteStatementWithoutResult(sql);
        }

        private void DropView(string schemaName, string viewName)
        {
            var sql = DropViewSql(schemaName, viewName);

            tester.ExecuteStatementWithoutResult(sql);
        }

        private void CreateTable(string schemaName, string tableName)
        {
            var sql = CreateTestTable(schemaName, tableName);

            tester.ExecuteStatementWithoutResult(sql);
        }

        private string DropTableSql(string schemaName, string tableName)
        {
            var template = "drop table if exists {0}.{1};";

            var sql = string.Format(template, schemaName, tableName);

            return sql;
        }

        private string DropViewSql(string schemaName, string viewName)
        {
            var template = "drop view if exists {0}.{1};";

            var sql = string.Format(template, schemaName, viewName);

            return sql;
        }

        private string CreateTestTable(string schemaName, string tableName)
        {
            var template = "create table {0}.{1}([col1] int);";

            var sql = string.Format(template, schemaName, tableName);

            return sql;
        }

        #endregion
    }
}
