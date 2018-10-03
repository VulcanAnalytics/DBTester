using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VulcanAnalytics.DBTester.Exceptions;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests
{
    [TestClass]
    public class DatabaseTesterTests
    {
        private const string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        private DatabaseTester mssqlTester = new DBTester.MsSqlDatabaseTester(connection);

        const string unavailableConnString = @"Data Source=(localdb)\blah;Initial Catalog=foobar;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=1;Encrypt=False;TrustServerCertificate=True";
        const string availableConnString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        private Type databasetesterType = typeof(DatabaseTester);

        [TestMethod]
        public void DatabaseTesterClass()
        {
            VulcanAnalytics.DBTester.DatabaseTester tester = null;
        }

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

        [TestMethod]
        public void HasMethodHasTable()
        {
            var methodFound = false;

            var methods = GetMethods(databasetesterType, "HasTable");
            if (methods.Length > 0)
            {
                methodFound = true;
            }

            Assert.IsTrue(methodFound, "Method of required name not found");
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
            var methodFound = false;

            var methods = GetMethods(databasetesterType, "ExecuteStatementWithoutResult");
            if (methods.Length > 0)
            {
                methodFound = true;
            }

            Assert.IsTrue(methodFound, "Method of required name not found");
        }
               
        [TestMethod]
        public void HasMethodRowCount()
        {
            var methodFound = false;

            var methods = GetMethods(databasetesterType, "RowCount");
            if (methods.Length > 0)
            {
                methodFound = true;
            }

            Assert.IsTrue(methodFound, "Method of required name not found");
        }

        [TestMethod]
        public void HasMethodInsertData()
        {
            var methodFound = false;

            var methods = GetMethods(databasetesterType, "InsertData");
            if (methods.Length > 0)
            {
                methodFound = true;
            }

            Assert.IsTrue(methodFound, "Method of required name not found");
        }

        [TestMethod]
        public void InsertDataCanInsertOneRow()
        {
            var expectedCount = 1;
            var connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=SSPI;";
            var tester = new DBTester.MsSqlDatabaseTester(connectionstring);
            tester.ExecuteStatementWithoutResult("drop table if exists [dbo].[testtable];");
            tester.ExecuteStatementWithoutResult("create table [dbo].[testtable]([col1] int);");
            var schemaName = "dbo";
            var tableName = "testtable";
            var columns = new string[]{ "col1" };
            var data = new object[]
                {
                    new Object[]{1}
                };


            tester.InsertData(schemaName, tableName, columns, data);


            var actualCount = tester.RowCount(schemaName, tableName);
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void InsertDataCanInsertMultipleRows()
        {
            var expectedCount = 2;
            var connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=SSPI;";
            var tester = new DBTester.MsSqlDatabaseTester(connectionstring);
            tester.ExecuteStatementWithoutResult("drop table if exists [dbo].[testtable];");
            tester.ExecuteStatementWithoutResult("create table [dbo].[testtable]([col1] int);");
            var schemaName = "dbo";
            var tableName = "testtable";
            var columns = new string[] { "col1" };
            var data = new object[]
                {
                    new Object[]{1},
                    new Object[]{1}
                };


            tester.InsertData(schemaName, tableName, columns, data);


            var actualCount = tester.RowCount(schemaName, tableName);
            Assert.AreEqual(expectedCount, actualCount);
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
        public void InsertDataCanInsertTextValues()
        {
            var expectedCount = 2;
            var connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=SSPI;";
            var tester = new DBTester.MsSqlDatabaseTester(connectionstring);
            tester.ExecuteStatementWithoutResult("drop table if exists [dbo].[testtable];");
            tester.ExecuteStatementWithoutResult("create table [dbo].[testtable]([col1] varchar(200));");
            var schemaName = "dbo";
            var tableName = "testtable";
            var columns = new string[] { "col1" };
            var data = new object[]
                {
                    new Object[]{1},
                    new Object[]{"Text"}
                };


            tester.InsertData(schemaName, tableName, columns, data);


            var actualCount = tester.RowCount(schemaName, tableName);
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void InsertDataCanInsertMultipleColumns()
        {
            var expectedCount = 2;
            var connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=SSPI;";
            var tester = new DBTester.MsSqlDatabaseTester(connectionstring);
            tester.ExecuteStatementWithoutResult("drop table if exists [dbo].[testtable];");
            tester.ExecuteStatementWithoutResult("create table [dbo].[testtable]([col1] varchar(200) not null, [col2] varchar(200) not null);");
            var schemaName = "dbo";
            var tableName = "testtable";
            var columns = new string[] { "col1","col2" };
            var data = new object[]
                {
                    new Object[]{1,"Two"},
                    new Object[]{"One",2}
                };


            tester.InsertData(schemaName, tableName, columns, data);


            var actualCount = tester.RowCount(schemaName, tableName);
            Assert.AreEqual(expectedCount, actualCount);
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
    }
}
