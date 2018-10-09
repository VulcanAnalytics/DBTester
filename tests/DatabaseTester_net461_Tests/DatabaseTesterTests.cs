using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests
{
    [TestClass]
    public class DatabaseTesterTests
    {
        private Type databasetesterType = typeof(DatabaseTester);

        [TestMethod]
        public void DatabaseTesterClass()
        {
            VulcanAnalytics.DBTester.DatabaseTester tester = null;
        }

        [TestMethod]
        public void HasMethodHasTable()
        {
            var methodName = "HasTable";

            var hasMethod = HasMethod(databasetesterType, methodName);

            Assert.IsTrue(hasMethod, "Method of required name not found");
        }

        [TestMethod]
        public void DatabaseTesterMethodHasTableReturnsBool()
        {
            Type returnType = null;

            var methods = GetMethods(databasetesterType, "HasTable");
            if (methods.Length > 0)
            {
                returnType = methods[0].ReturnType;
            }

            Assert.AreEqual(typeof(bool), returnType,"HasTable method doesn't return the correct type");
        }

        [TestMethod]
        public void HasMethodExecuteStatementWithoutResult()
        {
            var methodName = "ExecuteStatementWithoutResult";

            var hasMethod = HasMethod(databasetesterType, methodName);

            Assert.IsTrue(hasMethod, "Method of required name not found");
        }

        [TestMethod]
        public void HasMethodExecuteStatementWithResult()
        {
            var methodName = "ExecuteStatementWithResult";

            var hasMethod = HasMethod(databasetesterType, methodName);

            Assert.IsTrue(hasMethod, "Method of required name not found");
        }

        [TestMethod]
        public void ExecuteStatementWithResultReturnsDataSet()
        {
            Type returnType = null;

            var methods = GetMethods(databasetesterType, "ExecuteStatementWithResult");
            if (methods.Length > 0)
            {
                returnType = methods[0].ReturnType;
            }

            Assert.AreEqual(typeof(DataSet), returnType, "HasTable method doesn't return the correct type");
        }

        [TestMethod]
        public void HasMethodRowCount()
        {
            var methodName = "RowCount";

            var hasMethod = HasMethod(databasetesterType, methodName);

            Assert.IsTrue(hasMethod, "Method of required name not found");
        }

        [TestMethod]
        public void HasMethodInsertData()
        {
            var methodName = "InsertData";

            var hasMethod = HasMethod(databasetesterType, methodName);

            Assert.IsTrue(hasMethod, "Method of required name not found");
        }

        [TestMethod]
        public void InsertDataWikiExample()
        {
            // Setup the Database Tester object using the MS SQL implementation
            var connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=SSPI;";
            VulcanAnalytics.DBTester.DatabaseTester tester = new VulcanAnalytics.DBTester.MsSqlDatabaseTester(connectionstring);

            // Create a table to insert data into
            tester.ExecuteStatementWithoutResult("drop table if exists [dbo].[TestPerson];");
            tester.ExecuteStatementWithoutResult("create table [dbo].[TestPerson]([TestId] int, [Name] varchar(255), [ModifiedDate] datetime);");

            // Parameters for InsertData method
            var schemaName = "dbo";
            var tableName = "TestPerson";
            var columns = new string[] { "TestId", "Name", "ModifiedDate" };
            var data = new object[]
                {
                    new Object[]{1,"Joe","2018-09-28T12:49:13.576"},
                    new Object[]{2,"John","2018-10-01T18:31:29.256"}
                };

            // Call the Insertdata method to insert the rows
            tester.InsertData(schemaName, tableName, columns, data);
        }

        [TestMethod]
        public void InsertDataWithDefaultsWikiExample()
        {
            // Setup the Database Tester object using the MS SQL implementation
            var connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=SSPI;";
            VulcanAnalytics.DBTester.DatabaseTester tester = new VulcanAnalytics.DBTester.MsSqlDatabaseTester(connectionstring);

            // Create a table to insert data into
            tester.ExecuteStatementWithoutResult("drop table if exists [dbo].[TestPerson];");
            tester.ExecuteStatementWithoutResult("create table [dbo].[TestPerson]([TestId] int, [Name] varchar(255), [ModifiedDate] datetime, [ModifiedBy] sysname);");

            // Set the defaults for the insert
            var defaults = new ColumnDefaults();
            defaults.AddDefault(new KeyValuePair<string, object>("ModifiedBy", "DarrenComeau"));

            // Parameters for InsertData method
            var schemaName = "dbo";
            var tableName = "TestPerson";
            var columns = new string[] { "TestId", "Name", "ModifiedDate" };
            var data = new object[]
                {
                    new Object[]{1,"Joe","2018-09-28T12:49:13.576"},
                    new Object[]{2,"John","2018-10-01T18:31:29.256"}
                };

            // Call the Insertdata method to insert the rows
            tester.InsertData(schemaName, tableName, columns, data, defaults);
        }

        #region Private Methods
        private bool HasMethod(Type type, string methodName)
        {
            var hasMethod = false;

            var methods = GetMethods(type, methodName);
            if (methods.Length > 0)
            {
                hasMethod = true;
            }
            return hasMethod;
        }

        private MethodInfo[] GetMethods(Type type,string name)
        {
            List<MethodInfo> methods = new List<MethodInfo>();


            foreach (var method in type.GetMethods())
            {
                if (method.Name == name)
                {
                    methods.Add(method);
                }
            }

            return methods.ToArray();

        }
        #endregion
    }
}
