using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VulcanAnalytics.DBTester.Exceptions;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests.MsSqlDatabaseTester
{
    [TestClass]
    public class NewObject_Tests
    {
        private const string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        private DatabaseTester mssqlTester = new DBTester.MsSqlDatabaseTester(connection);

        const string unavailableConnString = @"Data Source=(localdb)\blah;Initial Catalog=foobar;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=1;Encrypt=False;TrustServerCertificate=True";
        const string availableConnString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        [TestMethod]
        public void ConstructorString()
        {
            try
            {
                var tester = new DBTester.MsSqlDatabaseTester(availableConnString);
            }
            catch { }
        }

        [TestMethod]
        public void ConstructorRejectsInvalidConnectionString()
        {
            try
            {
                var tester = new DBTester.MsSqlDatabaseTester("not a connection string");

                Assert.Fail();
            }
            catch (AssertFailedException)
            {
                Assert.Fail("Invalid connection string accepted");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(FailedDatabaseConnection));
            }
        }

        [TestMethod]
        public void ConstructorRejectsUnavailableConnection()
        {
            try
            {
                var tester = new DBTester.MsSqlDatabaseTester(unavailableConnString);

                Assert.Fail();
            }
            catch (AssertFailedException)
            {
                Assert.Fail("Unavailable connection string accepted");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(FailedDatabaseConnection));
            }
        }
    }
}
