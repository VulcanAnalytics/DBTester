using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests.MsSqlDatabaseTester
{
    [TestClass]
    public class ExecuteStatementWithoutResult_Tests
    {
        private const string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        private DatabaseTester tester = new VulcanAnalytics.DBTester.MsSqlDatabaseTester(connection);


        [TestMethod]
        public void StatementExecutes()
        {
            var sql = "drop table if exists [dbo].[testtable]; create table [dbo].[testtable]([col1] int);";

            tester.ExecuteStatementWithoutResult(sql);

            var statementSuccessful = tester.HasTable("dbo","testtable");
            Assert.IsTrue(statementSuccessful);
        }
    }
}
