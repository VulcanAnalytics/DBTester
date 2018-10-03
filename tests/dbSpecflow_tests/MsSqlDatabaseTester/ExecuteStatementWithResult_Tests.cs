using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests.MsSqlDatabaseTester
{
    [TestClass]
    public class ExecuteStatementWithResult_Tests
    {
        private const string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        private DatabaseTester tester = new VulcanAnalytics.DBTester.MsSqlDatabaseTester(connection);


        [TestMethod]
        public void StatementExecutes()
        {
            var sql = "select 'Hello, World' as [Text];";

            var results = tester.ExecuteStatementWithResult(sql);

            Assert.IsNotNull(results);
        }


        [TestMethod]
        public void ReturnsCorrectNumberOfTables()
        {
            var expectedTableCount = 1;
            var sql = "select 'Hello, World' as [Text];";

            var results = tester.ExecuteStatementWithResult(sql);

            var actualTableCount = results.Tables.Count;
            Assert.AreEqual(expectedTableCount,actualTableCount);
        }


        [TestMethod]
        public void ReturnsCorrectData()
        {
            var expectedText = "Hello, World";
            var sql = "select 'Hello, World' as [Text];";

            var results = tester.ExecuteStatementWithResult(sql);

            var actualText = results.Tables[0].Rows[0]["Text"];
            Assert.AreEqual(expectedText, actualText);
        }
    }
}
