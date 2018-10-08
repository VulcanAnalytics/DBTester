using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests.DatabaseTester_Tests
{
    [TestClass]
    public class ColumnDefaults_Tests
    {
        [TestMethod]
        public void New_ColumnDefaults_Are_Empty()
        {
            var columnDefaults = new ColumnDefaults();

            Assert.IsTrue(columnDefaults.IsEmpty);
        }

        [TestMethod]
        public void ColumnDefaults_When_Assigned_Is_Not_Empty()
        {
            var columnDefaults = new ColumnDefaults();
            columnDefaults.AddDefault(new KeyValuePair<string, object>("Hello", "World"));

            Assert.IsFalse(columnDefaults.IsEmpty);
        }

        [TestMethod]
        public void I_Can_Add_And_Retrieve_A_Column_Value_Pair()
        {
            var columnDefaults = new ColumnDefaults();

            var columnDefault = new KeyValuePair<string,object>("Hello","World");

            columnDefaults.AddDefault(columnDefault);

            foreach (var defaultColumn in columnDefaults)
            {
                Assert.AreEqual(columnDefault, defaultColumn);
            }
        }

        [TestMethod]
        public void I_Can_Loop_Over_All_Defaults()
        {
            var expectedCount = 4;
            var columnDefaults = new ColumnDefaults();
            var i = 0;
            while (i < expectedCount)
            {
                var keyString = string.Format("Hello {0}", i);
                columnDefaults.AddDefault(new KeyValuePair<string, object>(keyString, "World"));
                i++;
            }


            var actualCount = 0;
            foreach (var columnDefault in columnDefaults)
            {
                actualCount++;
            }


            Assert.AreEqual(expectedCount,actualCount);
        }

        [ExpectedException(typeof(VulcanAnalytics.DBTester.Exceptions.ColumnDefaultAlreadyAdded))]
        [TestMethod]
        public void I_Cannot_Add_Duplicate_Column_Value_Pair()
        {
            var columnDefaults = new ColumnDefaults();

            var columnDefault = new KeyValuePair<string, object>("Hello", "World");

            columnDefaults.AddDefault(columnDefault);

            columnDefaults.AddDefault(columnDefault);
        }
    }
}
