using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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

        [TestMethod]
        public void InsertDataCanInsertNullColumns()
        {
            var expectedCount = 2;
            var schemaName = "dbo";
            var tableName = "testtable";
            DropAndCreateTestTable(schemaName, tableName, "[col1] varchar(200), [col2] int");
            var columns = new string[] { "col1", "col2" };
            var data = new object[]
                {
                    new object[]{null as string,2},
                    new object[]{"One",null as int?}
                };


            tester.InsertData(schemaName, tableName, columns, data);


            var actualCount = tester.RowCount(schemaName, tableName);
            var results = tester.ExecuteStatementWithResult("select [col1],[col2] from [dbo].[testtable];");
            Assert.AreEqual(expectedCount, actualCount);
            Assert.IsInstanceOfType(results.Tables[0].Rows[0]["col1"], typeof(System.DBNull));
            Assert.IsInstanceOfType(results.Tables[0].Rows[1]["col2"], typeof(System.DBNull));
        }

        [TestMethod]
        public void I_Can_Insert_Some_Columns_With_Defaults_For_Other_Columns()
        {
            var expectedValue = "Hello, World.";
            var schemaName = "dbo";
            var tableName = "testtable";
            DropAndCreateTestTable(schemaName, tableName, "[col1manual] int, [col2withdefault] varchar(200)");
            var columns = new string[] { "col1manual" };
            var data = new object[]
                {
                    new object[]{1}
                };
            var defaults = new ColumnDefaults();
            defaults.AddDefault(new KeyValuePair<string, object>("col2withdefault", expectedValue));


            tester.InsertData(schemaName, tableName, columns, data, defaults);


            var results = tester.ExecuteStatementWithResult(string.Format("select * from {0}.{1};",schemaName,tableName));
            var actualValue = results.Tables[0].Rows[0]["col2withdefault"];
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void Data_Specified_For_A_Column_Overrides_The_Default_Data()
        {
            var expectedValue = "Hello, World.";
            var schemaName = "dbo";
            var tableName = "testtable";
            DropAndCreateTestTable(schemaName, tableName, "[col1manual] int, [col2withdefault] varchar(200)");
            var columns = new string[] { "col1manual", "col2withdefault" };
            var data = new object[]
                {
                    new object[]{ 1, expectedValue }
                };
            var defaults = new ColumnDefaults();
            defaults.AddDefault(new KeyValuePair<string, object>("col2withdefault", "Column's default value"));


            tester.InsertData(schemaName, tableName, columns, data, defaults);


            var results = tester.ExecuteStatementWithResult(string.Format("select * from {0}.{1};", schemaName, tableName));
            var actualValue = results.Tables[0].Rows[0]["col2withdefault"];
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void Inserting_Data_Correctly_Handles_Single_Quotes()
        {
            var expectedValue = "Hello, 'World'.";
            var schemaName = "dbo";
            var tableName = "testtable";
            DropAndCreateTestTable(schemaName, tableName, "[col1] varchar(200)");
            var columns = new string[] { "col1" };
            var data = new object[]
                {
                    new object[]{ expectedValue }
                };


            tester.InsertData(schemaName, tableName, columns, data);


            var results = tester.ExecuteStatementWithResult(string.Format("select * from {0}.{1};", schemaName, tableName));
            var actualValue = results.Tables[0].Rows[0]["col1"];
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void Inserting_Defaults_Correctly_Handles_Single_Quotes()
        {
            var expectedValue = "Hello, 'World'.";
            var schemaName = "dbo";
            var tableName = "testtable";
            DropAndCreateTestTable(schemaName, tableName, "[col1manual] int, [col2withdefault] varchar(200)");
            var columns = new string[] { "col1manual" };
            var data = new object[]
                {
                    new object[]{1}
                };
            var defaults = new ColumnDefaults();
            defaults.AddDefault(new KeyValuePair<string, object>("col2withdefault", expectedValue));


            tester.InsertData(schemaName, tableName, columns, data, defaults);


            var results = tester.ExecuteStatementWithResult(string.Format("select * from {0}.{1};", schemaName, tableName));
            var actualValue = results.Tables[0].Rows[0]["col2withdefault"];
            Assert.AreEqual(expectedValue, actualValue);
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