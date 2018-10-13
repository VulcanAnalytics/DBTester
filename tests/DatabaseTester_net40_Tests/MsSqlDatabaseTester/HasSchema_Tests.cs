using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace VulcanAnalytics.DBTester.dbSpecflow_tests.MsSqlDatabaseTester
{
    [TestClass]
    public class HasSchema_Tests
    {
        private const string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        private DatabaseTester tester = new VulcanAnalytics.DBTester.MsSqlDatabaseTester(connection);

        [TestMethod]
        public void ReturnsFalseWhenSchemaDoesntExist()
        {
            var schemaName = "unknownSchema";

            var hasSchema = tester.HasSchema(schemaName);

            Assert.IsFalse(hasSchema);
        }

        [TestMethod]
        public void ReturnsTrueWhenSchemaDoesExist()
        {
            var schemaName = "dbo";

            var hasSchema = tester.HasSchema(schemaName);

            Assert.IsTrue(hasSchema);
        }
    }
}
