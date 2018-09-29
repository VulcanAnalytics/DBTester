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
        private const string viewName = "testview";

        [TestMethod]
        public void RowCountReturnsNumberOfRowsFromTable()
        {
            var expectedCount = 5;
            CreateAndPopulateTable(schemaName, tableName, expectedCount);

            var actualCount = tester.RowCount(schemaName, tableName);

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void RowCountReturnsNumberOfRowsFromView()
        {
            var expectedCount = 5;
            CreateAndPopulateTable(schemaName, tableName, expectedCount);
            DropAndCreateView(schemaName, tableName, viewName);

            var actualCount = tester.RowCount(schemaName, viewName);

            Assert.AreEqual(expectedCount, actualCount);
        }

        #region Private Methods

        private void CreateAndPopulateTable(string schemaName, string tableName, int rowCount)
        {
            DropAndCreateTable(schemaName, tableName);
            InsertTestRows(schemaName, tableName, rowCount);
        }

        private void DropAndCreateTable(string schemaName, string tableName)
        {
            DropTable(schemaName, tableName);
            CreateTable(schemaName, tableName);
        }

        private void DropAndCreateView(string schemaName, string tableName, string viewName)
        {
            DropView(schemaName, viewName);
            CreateView(schemaName, tableName, viewName);
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
            var sql = CreateTestTableSql(schemaName, tableName);

            tester.ExecuteStatementWithoutResult(sql);
        }

        private void CreateView(string schemaName, string tableName, string viewName)
        {
            var sql = CreateTestViewSql(schemaName, tableName, viewName);

            tester.ExecuteStatementWithoutResult(sql);
        }

        private void InsertTestRows(string schemaName, string tableName, int rowCount)
        {
            var sql = InsertTestRowSql(schemaName, tableName);
            var i = 0;
            while (i < rowCount)
            {
                tester.ExecuteStatementWithoutResult(sql);
                i++;
            }
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

        private string CreateTestTableSql(string schemaName, string tableName)
        {
            var template = "create table {0}.{1}([col1] int);";

            var sql = string.Format(template, schemaName, tableName);

            return sql;
        }

        private string CreateTestViewSql(string schemaName, string tableName, string viewName)
        {
            var template = "create view {0}.{1} as select [col1] from {0}.{2};";

            var sql = string.Format(template, schemaName, viewName, tableName);

            return sql;
        }

        private string InsertTestRowSql(string schemaName, string tableName)
        {
            var template = "insert into {0}.{1}([col1]) values(99);";

            var sql = string.Format(template, schemaName, tableName);

            return sql;
        }

        #endregion
    }
}
