namespace VulcanAnalytics.DBTester
{
    public class MsSqlDatabaseTester : DatabaseTester
    {
        public MsSqlDatabaseTester(string connectionString) : base(connectionString)
        {

        }
        
        public override bool HasSchema(string schemaName)
        {
            return base.database.Schemas.Contains(schemaName);
        }

        public override bool HasTable(string schemaName, string tableName)
        {
            return base.database.Tables.Contains(tableName, schemaName);
        }

        public override bool HasTable(string tableName)
        {
            return HasTable(base.defaultSchema, tableName);
        }
}
}
