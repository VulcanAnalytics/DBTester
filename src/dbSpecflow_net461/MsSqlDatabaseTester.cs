namespace VulcanAnalytics.DBTester
{
    public class MsSqlDatabaseTester : DatabaseTester
    {
        public MsSqlDatabaseTester( string connectionString) : base(connectionString)
        {

        }
        
        public override bool HasSchema(string schemaName)
        {
            return database.Schemas.Contains(schemaName);
        }
    }
}
