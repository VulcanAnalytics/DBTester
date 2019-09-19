using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VulcanAnalytics.DBTester.Exceptions;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests.DatabaseTester_Tests
{
    [TestClass]
    public class ObjectData_Tests
    {
        private const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=SSPI;";
        private DatabaseTester tester = new Mocks.MockDatabaseTester(connectionString);

        [TestInitialize]
        public void Initialize()
        {
            DropTable("dbo", "testtable");
        }

        [TestMethod]
        public void ObjectData_Retrieves_All_Rows_From_Table()
        {
            var expectedCount = 3;
            var schemaName = "dbo";
            var objectName = "testtable";
            CreateAndPopulateTable(schemaName, objectName, expectedCount);


            var results = tester.ObjectData(schemaName, objectName);


            Assert.AreEqual(expectedCount, results.Rows.Count);
        }

        [TestMethod]
        public void ObjectData_Retrieves_Zero_Rows_From_Empty_Table()
        {
            var expectedCount = 0;
            var schemaName = "dbo";
            var objectName = "testtable";
            CreateAndPopulateTable(schemaName, objectName, expectedCount);


            var results = tester.ObjectData(schemaName, objectName);


            Assert.AreEqual(expectedCount, results.Rows.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFound))]
        public void When_I_Retrieve_Data_From_A_Missing_Object_Get_An_Error()
        {
            var schemaName = "dbo";
            var objectName = "testtable";

            DropTable(schemaName, objectName);

            var results = tester.ObjectData(schemaName, objectName);
        }

        [TestMethod]
        public void ObjectData_Retrieves_All_Columns_From_A_Table()
        {
            var schemaName = "dbo";
            var objectName = "testtable";
            CreateTable(schemaName, objectName);


            var results = tester.ObjectData(schemaName, objectName);


            Assert.AreEqual(3, results.Columns.Count);
        }


        #region Private Methods

        private void CreateAndPopulateTable(string schemaName, string tableName, int rowCount)
        {
            DropAndCreateTable(schemaName, tableName);
            InsertTestRows(schemaName, tableName, rowCount);
        }

        private void DropAndCreateTable(string schemaName, string tableName)
        {
            DropTable(schemaName, tableName);
            CreateTable(schemaName, tableName);
        }

        private void DropAndCreateView(string schemaName, string tableName, string viewName)
        {
            DropView(schemaName, viewName);
            CreateView(schemaName, tableName, viewName);
        }

        private void DropTable(string schemaName, string tableName)
        {
            var sql = DropTableSql(schemaName, tableName);

            tester.ExecuteStatementWithoutResult(sql);
        }

        private void DropView(string schemaName, string viewName)
        {
            var sql = DropViewSql(schemaName, viewName);

            tester.ExecuteStatementWithoutResult(sql);
        }

        private void CreateTable(string schemaName, string tableName)
        {
            var sql = CreateTestTableSql(schemaName, tableName);

            tester.ExecuteStatementWithoutResult(sql);
        }

        private void CreateView(string schemaName, string tableName, string viewName)
        {
            var sql = CreateTestViewSql(schemaName, tableName, viewName);

            tester.ExecuteStatementWithoutResult(sql);
        }

        private void InsertTestRows(string schemaName, string tableName, int rowCount)
        {
            var sql = InsertTestRowSql(schemaName, tableName);
            var i = 0;
            while (i < rowCount)
            {
                tester.ExecuteStatementWithoutResult(sql);
                i++;
            }
        }

        private string DropTableSql(string schemaName, string tableName)
        {
            var template = "drop table if exists {0}.{1};";

            var sql = string.Format(template, schemaName, tableName);

            return sql;
        }

        private string DropViewSql(string schemaName, string viewName)
        {
            var template = "drop view if exists {0}.{1};";

            var sql = string.Format(template, schemaName, viewName);

            return sql;
        }

        private string CreateTestTableSql(string schemaName, string tableName)
        {
            var template = "create table {0}.{1}([col1] int, [col2] int, [col3] int);";

            var sql = string.Format(template, schemaName, tableName);

            return sql;
        }

        private string CreateTestViewSql(string schemaName, string tableName, string viewName)
        {
            var template = "create view {0}.{1} as select [col1] from {0}.{2};";

            var sql = string.Format(template, schemaName, viewName, tableName);

            return sql;
        }

        private string InsertTestRowSql(string schemaName, string tableName)
        {
            var template = "insert into {0}.{1}([col1]) values(99);";

            var sql = string.Format(template, schemaName, tableName);

            return sql;
        }

        #endregion
    }
}
