using Microsoft.VisualStudio.TestTools.UnitTesting;
using VulcanAnalytics.DBTester.Exceptions;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests.MsSqlDatabaseTester
{
    [TestClass]
    public class NewObject_Tests
    {        
        const string unavailableConnString = @"Data Source=(localdb)\blah;Initial Catalog=foobar;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=1;Encrypt=False;TrustServerCertificate=True";
        const string availableConnString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";


        [TestMethod]
        public void NewObjectAcceptsGoodConnection()
        {
            var tester = new DBTester.MsSqlDatabaseTester(availableConnString);
        }

        [TestMethod]
        [ExpectedException(typeof(FailedDatabaseConnection))]
        public void NewObjectRejectsInvalidConnectionString()
        {
            var sut = new DBTester.MsSqlDatabaseTester("not a connection string");
        }

        [TestMethod]
        [ExpectedException(typeof(FailedDatabaseConnection))]
        public void NewObjectRejectsUnavailableConnection()
        {
            var sut = new DBTester.MsSqlDatabaseTester(unavailableConnString);
        }
    }
}
