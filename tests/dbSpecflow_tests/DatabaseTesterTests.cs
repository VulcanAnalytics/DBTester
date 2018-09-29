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

        const string unavailableConnString = @"Data Source=(localdb)\blah;Initial Catalog=foobar;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";
        const string availableConnString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        private Type databasetesterType = typeof(DatabaseTester);

        [TestMethod]
        public void DatabaseTesterClass()
        {
            VulcanAnalytics.DBTester.DatabaseTester tester = null;
        }

        [TestMethod]
        public void DatabaseTesterConstructorString()
        {
            try
            {
                var tester = new DBTester.MsSqlDatabaseTester(availableConnString);
            }
            catch { }
        }

        [TestMethod]
        public void DatabaseTesterConstructorRejectsInvalidConnectionString()
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
                Assert.IsInstanceOfType(e, typeof(DatabaseTesterConnectionException));
            }
        }

        [TestMethod]
        public void DatabaseTesterConstructorRejectsUnavailableConnection()
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
                Assert.IsInstanceOfType(e, typeof(DatabaseTesterConnectionException));
            }
        }

        [TestMethod]
        public void DatabaseTesterHasMethodHasTable()
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
        public void DatabaseTesterHasMethodExecuteStatementWithoutResult()
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
        public void DatabasetesterHasMethodRowCount()
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
        public void RowCountReturnsNumberOfRowsFromTable()
        {
            var expectedCount = 5;
            var tester = new DBTester.MsSqlDatabaseTester(availableConnString);
            tester.ExecuteStatementWithoutResult("drop table if exists [dbo].[testtable];");
            tester.ExecuteStatementWithoutResult("create table [dbo].[testtable]([col1] int);");
            var i = 0;
            while (i < expectedCount)
            {
                tester.ExecuteStatementWithoutResult("insert into [dbo].[testtable]([col1]) values(99);");
                i++;
            }

            var actualCount = tester.RowCount("dbo", "testtable");

            Assert.AreEqual(expectedCount,actualCount);
        }

        [TestMethod]
        public void RowCountReturnsNumberOfRowsFromView()
        {
            var expectedCount = 5;
            var tester = new DBTester.MsSqlDatabaseTester(availableConnString);
            tester.ExecuteStatementWithoutResult("drop table if exists [dbo].[testtable];");
            tester.ExecuteStatementWithoutResult("create table [dbo].[testtable]([col1] int);");
            var i = 0;
            while (i < expectedCount)
            {
                tester.ExecuteStatementWithoutResult("insert into [dbo].[testtable]([col1]) values(99);");
                i++;
            }
            tester.ExecuteStatementWithoutResult("drop view if exists [dbo].[testview];");
            tester.ExecuteStatementWithoutResult("create view [dbo].[testview] as select [col1] from [dbo].[testtable];");

            var actualCount = tester.RowCount("dbo", "testview");

            Assert.AreEqual(expectedCount, actualCount);
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
