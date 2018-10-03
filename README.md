# DBTester

### What is DBTester?

DBTester is a small library built to remove the need for database plumbing and repetitive code for performing utility actions from one test project to the next. Afterall why copy and paste the same database code from one project to the next, or worse still, write it out again?

This library is available through [NuGet](https://www.nuget.org/packages/VulcanAnalytics.DBTester) for immediate inclusion in your projects

### Ok, but what can I actually do with DBTester?

First, create an instance of DBTester with the MSSQL database you wish to interact with;

```csharp
using VulcanAnalytics.DBTester;
```

```csharp
var connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";
DatabaseTester tester = new MsSqlDatabaseTester(connectionstring);
```

Secondly, use one or more of the functions to interact with or check a condition from the database;

```csharp
var schema = "dbo";
var table = "testtable";

var hasTable = tester.HasTable(schema, table);

Assert.IsTrue(hasTable);
```

And there we have it, no need to decide on how to use various other database libraries or SQL scripts, just one succint place to come to for database testing utilities.

This list of methods is small today, but increasing (as the database project I'm currently working on requires further unit testing).
* HasSchema
* HasTable
* RowCount
* InsertData
* ExecuteStatementWithoutResult
* ExecuteStatementWithResult

### Where did this come from?

This project has been born of the experience I have had in writing the same code for database unit and user acceptance testing in MSTest and SpecFlow. DBTester marks the third generation of this effort (both of the previous generations were client's closed source implementations).
In previous implementations there have been functionality for interacting with SQL Server Agent and SQL Server Integration Services revealing my background as a Business Intelligence and Data Warehousing Engineer.

### Now where is it going?

My roadmap for DBTester is to work towards a minimally functional yet stable release for version 1.0.0.

A few of the key aspects to have in place are;
* high test coverage of the library
* continuous integration and delivery
* clear documentation on the wiki
* NuGet packaging
* Abstract class to support extensions for other databases/techniques

Much of this is already in place via Microsoft DevOps (formally Visual Studio Team Services)

With this in place over the coming month or so (October 2018) - I would then be welcoming the idea of other contributors being involved.

### Where can I get it?

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [DBTester](https://www.nuget.org/packages/VulcanAnalytics.DBTester/) from the package manager console:

```
PM> Install-Package VulcanAnalytics.DBTester
```

### Do you have an issue?

If you're running into problems, please file an issue here on GitHub at the top of the page.

### License, etc.

DBTester is Copyright &copy; 2018 Darren Comeau and other contributors under the [GNU GPLv3 License](LICENSE.txt).
