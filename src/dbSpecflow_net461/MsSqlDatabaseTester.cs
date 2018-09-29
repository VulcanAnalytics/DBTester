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

        public override int RowCount(string schemaName, string objectName)
        {
            var sqlStatement = string.Format("select count(*) from {0}.{1};", schemaName, objectName);

            var results = database.ExecuteWithResults(sqlStatement);

            var count = int.Parse(results.Tables[0].Rows[0][0].ToString());

            return count;
        }
    }
}
