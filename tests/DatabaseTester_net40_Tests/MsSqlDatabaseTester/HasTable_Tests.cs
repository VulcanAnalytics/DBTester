using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests.MsSqlDatabaseTester
{
    [TestClass]
    public class HasTable_Tests
    {
        private const string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        private DatabaseTester tester = new VulcanAnalytics.DBTester.MsSqlDatabaseTester(connection);

        private const string schemaName = "dbo";
        private const string tableName = "testtable";



        [TestMethod]
        public void ReturnsFalseWhenSchemaDoesntExist()
        {
            var schemaName = "unknownSchema";

            var hasTable = tester.HasTable(schemaName, tableName);

            Assert.IsFalse(hasTable);
        }

        [TestMethod]
        public void ReturnsFalseWhenTableDoesntExist()
        {
            DropTable(schemaName, tableName);

            var hasTable = tester.HasTable(schemaName, tableName);

            Assert.IsFalse(hasTable);
        }

        [TestMethod]
        public void ReturnsTrueWhenTableExists()
        {
            DropAndCreateTable(schemaName, tableName);

            var hasTable = tester.HasTable(schemaName,tableName);

            Assert.IsTrue(hasTable);
        }

        [TestMethod]
        public void ReturnsFalseWhenTableDoesntExistInDefaultSchema()
        {
            var defaultSchema = "dbo";
            DropTable(defaultSchema, tableName);

            var hasTable = tester.HasTable(tableName);

            Assert.IsFalse(hasTable);
        }

        [TestMethod]
        public void ReturnsTrueWhenTableExistsInDefaultSchema()
        {
            var defaultSchema = "dbo";
            DropAndCreateTable(defaultSchema,tableName);

            var hasTable = tester.HasTable(tableName);

            Assert.IsTrue(hasTable);
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

        private string CreateTestTable(string schemaName, string tableName)
        {
            var template = "create table {0}.{1}([col1] int);";

            var sql = string.Format(template, schemaName, tableName);

            return sql;
        }

        #endregion
    }
}
