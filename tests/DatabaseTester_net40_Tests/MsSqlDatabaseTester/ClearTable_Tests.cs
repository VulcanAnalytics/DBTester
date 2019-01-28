using Microsoft.VisualStudio.TestTools.UnitTesting;
using VulcanAnalytics.DBTester.Exceptions;

namespace VulcanAnalytics.DBTester.dbSpecflow_tests.MsSqlDatabaseTester
{
    [TestClass]
    public class ClearTable_Tests
    {
        private const string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        private DatabaseTester tester = new VulcanAnalytics.DBTester.MsSqlDatabaseTester(connection);

        private const string schemaName = "dbo";
        private const string tableName = "testtable";

        [TestMethod]
        public void Will_Remove_Data_If_Table_Has_Rows()
        {
            CreateAndPopulateTable(schemaName, tableName, 5);

            tester.ClearTable(schemaName, tableName);

            Assert.AreEqual(0,tester.RowCount(schemaName, tableName));
        }

        [TestMethod]
        public void Will_Remove_Data_From_Table_Which_Has_Space_In_Name_Has_Rows()
        {
            var tableName = "My Space Table";
            CreateAndPopulateTable(schemaName, tableName, 5);

            tester.ClearTable(schemaName, tableName);

            Assert.AreEqual(0, tester.RowCount(schemaName, tableName));
        }

        [TestMethod]
        public void Will_Remove_Data_From_Table_Which_Has_Space_In_Name_Has_Rows_And_Another_Shorter_Named_Table_With_Same_Start()
        {
            var tableName = "My Space Table";
            CreateAndPopulateTable(schemaName, tableName, 5);
            DropAndCreateTable(schemaName, "My Space"); // clashing table name

            Assert.AreEqual(5, tester.RowCount(schemaName, tableName));

            tester.ClearTable(schemaName, tableName);

            Assert.AreEqual(0, tester.RowCount(schemaName, tableName));
        }

        [TestMethod]
        public void Will_Do_Nothing_If_Table_Is_Empty()
        {
            DropAndCreateTable(schemaName, tableName);

            tester.ClearTable(schemaName, tableName);

            Assert.AreEqual(0, tester.RowCount(schemaName, tableName));
        }

        [TestMethod]
        public void Will_Clear_Data_From_Table_With_Foreign_Key()
        {
            tester.DropTable("dbo", "child");
            tester.DropTable("dbo", "parent");
            tester.ExecuteStatementWithoutResult("create table [dbo].[parent]([id] int primary key, [name] varchar(200));");
            tester.ExecuteStatementWithoutResult("create table [dbo].[child]([id] int, [parentid] int not null, [name] varchar(200));");
            tester.ExecuteStatementWithoutResult("alter table [dbo].[child] add constraint [FK_parent_child] foreign key ([parentid]) references [dbo].[parent]([id]);");
            tester.ExecuteStatementWithoutResult("insert into [dbo].[parent]([id],[name]) values (1,'testparent');");

            
            tester.ClearTable("dbo", "parent");


            Assert.AreEqual(0, tester.RowCount("dbo", "parent"));
        }

        [TestMethod]
        public void Will_Clear_Data_From_Table_With_Foreign_Key_And_Space_In_Object_Name()
        {
            tester.DropTable("dbo", "child");
            tester.DropTable("dbo", "spaced out parent");
            tester.ExecuteStatementWithoutResult("create table [dbo].[spaced out parent]([id] int primary key, [name] varchar(200));");
            tester.ExecuteStatementWithoutResult("create table [dbo].[child]([id] int, [parentid] int not null, [name] varchar(200));");
            tester.ExecuteStatementWithoutResult("alter table [dbo].[child] add constraint [FK_parent_child] foreign key ([parentid]) references [dbo].[spaced out parent]([id]);");
            tester.ExecuteStatementWithoutResult("insert into [dbo].[spaced out parent]([id],[name]) values (1,'testparent');");


            tester.ClearTable("dbo", "spaced out parent");


            Assert.AreEqual(0, tester.RowCount("dbo", "spaced out parent"));
        }

        [TestMethod]
        [ExpectedException(typeof(ChildTablesWithDataReferenceThisTable))]
        public void Will_Error_If_Table_With_Foreign_Key_Has_Cascaded_Data()
        {
            tester.DropTable("dbo", "child");
            tester.DropTable("dbo", "parent");
            tester.ExecuteStatementWithoutResult("create table [dbo].[parent]([id] int primary key, [name] varchar(200));");
            tester.ExecuteStatementWithoutResult("create table [dbo].[child]([id] int, [parentid] int not null, [name] varchar(200));");
            tester.ExecuteStatementWithoutResult("alter table [dbo].[child] add constraint [FK_parent_child] foreign key ([parentid]) references [dbo].[parent]([id]);");
            tester.ExecuteStatementWithoutResult("insert into [dbo].[parent]([id],[name]) values (1,'testparent');");
            tester.ExecuteStatementWithoutResult("insert into [dbo].[child]([id],[parentid],[name]) values (1,1,'testchild');");


            tester.ClearTable("dbo", "parent");


            Assert.AreEqual(1, tester.RowCount("dbo", "child"));
            Assert.AreEqual(1, tester.RowCount("dbo", "parent"));
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
            var template = "drop table if exists [{0}].[{1}];";

            var sql = string.Format(template, schemaName, tableName);

            return sql;
        }

        private string DropViewSql(string schemaName, string viewName)
        {
            var template = "drop view if exists [{0}].[{1}];";

            var sql = string.Format(template, schemaName, viewName);

            return sql;
        }

        private string CreateTestTableSql(string schemaName, string tableName)
        {
            var template = "create table [{0}].[{1}]([col1] int);";

            var sql = string.Format(template, schemaName, tableName);

            return sql;
        }

        private string CreateTestViewSql(string schemaName, string tableName, string viewName)
        {
            var template = "create view [{0}].[{1}] as select [col1] from [{0}].[{2}];";

            var sql = string.Format(template, schemaName, viewName, tableName);

            return sql;
        }

        private string InsertTestRowSql(string schemaName, string tableName)
        {
            var template = "insert into [{0}].[{1}]([col1]) values(99);";

            var sql = string.Format(template, schemaName, tableName);

            return sql;
        }

        #endregion
    }
}
