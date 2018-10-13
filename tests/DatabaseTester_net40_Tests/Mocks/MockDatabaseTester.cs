namespace VulcanAnalytics.DBTester.dbSpecflow_tests.Mocks
{
    public class MockDatabaseTester : VulcanAnalytics.DBTester.MsSqlDatabaseTester
    {
        public MockDatabaseTester(string connectionString) : base(connectionString)
        {
        }
    }
}
