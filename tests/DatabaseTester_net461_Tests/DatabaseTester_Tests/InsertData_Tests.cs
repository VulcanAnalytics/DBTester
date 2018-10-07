﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests.DatabaseTester_Tests
{
    [TestClass]
    public class InsertData_Tests
    {
        private const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=SSPI;";
        private DatabaseTester tester = new Mocks.MockDatabaseTester(connectionString);

        [TestMethod]
        public void InsertDataCanInsertOneRow()
        {
            var expectedCount = 1;
            var schemaName = "dbo";
            var tableName = "testtable";
            DropAndCreateTestTable(schemaName, tableName, "[col1] int");
            var columns = new string[] { "col1" };
            var data = new object[]
                {
                    new object[]{1}
                };


            tester.InsertData(schemaName, tableName, columns, data);


            var actualCount = tester.RowCount(schemaName, tableName);
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void InsertDataCanInsertMultipleRows()
        {
            var expectedCount = 2;
            var schemaName = "dbo";
            var tableName = "testtable";
            DropAndCreateTestTable(schemaName, tableName, "[col1] int");
            var columns = new string[] { "col1" };
            var data = new object[]
                {
                    new object[]{1},
                    new object[]{1}
                };


            tester.InsertData(schemaName, tableName, columns, data);


            var actualCount = tester.RowCount(schemaName, tableName);
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void InsertDataCanInsertTextValues()
        {
            var expectedCount = 2;
            var schemaName = "dbo";
            var tableName = "testtable";
            DropAndCreateTestTable(schemaName, tableName, "[col1] varchar(200)");
            var columns = new string[] { "col1" };
            var data = new object[]
                {
                    new object[]{1},
                    new object[]{"Text"}
                };


            tester.InsertData(schemaName, tableName, columns, data);


            var actualCount = tester.RowCount(schemaName, tableName);
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void InsertDataCanInsertMultipleColumns()
        {
            var expectedCount = 2;
            var schemaName = "dbo";
            var tableName = "testtable";
            DropAndCreateTestTable(schemaName, tableName, "[col1] varchar(200) not null, [col2] varchar(200) not null");
            var columns = new string[] { "col1", "col2" };
            var data = new object[]
                {
                    new object[]{1,"Two"},
                    new object[]{"One",2}
                };


            tester.InsertData(schemaName, tableName, columns, data);


            var actualCount = tester.RowCount(schemaName, tableName);
            Assert.AreEqual(expectedCount, actualCount);
        }

        #region Private Methods
        private void DropAndCreateTestTable(string schemaName, string tableName, string columnDef)
        {
            DropTable(schemaName, tableName);
            CreateTestTable(schemaName, tableName, columnDef);
        }

        private void DropTable(string schemaName, string tableName)
        {
            var dropTableStatement = string.Format("drop table if exists {0}.{1};", schemaName, tableName);
            tester.ExecuteStatementWithoutResult(dropTableStatement);
        }

        private void CreateTestTable(string schemaName, string tableName, string columnDef)
        {
            var createTableStatement = string.Format("create table {0}.{1}({2});",schemaName,tableName,columnDef);
            tester.ExecuteStatementWithoutResult(createTableStatement);
        }
        #endregion
    }
}