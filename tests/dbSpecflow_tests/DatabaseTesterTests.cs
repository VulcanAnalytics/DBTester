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
                var tester = new DatabaseTester(availableConnString);
            }
            catch { }
        }

        [TestMethod]
        public void DatabaseTesterConstructorRejectsInvalidConnectionString()
        {
            try
            {
                var tester = new DatabaseTester("not a connection string");

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
                var tester = new DatabaseTester(unavailableConnString);

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
        public void DatabasetesterHasHasTableMethod()
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
        public void DatabaseTesterHasSchemaDboReturnsTrue()
        {
            var connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=SSPI;";

            var dbTester = new DatabaseTester(connectionstring);

            Assert.IsTrue(dbTester.HasSchema("dbo"));
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
