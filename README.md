# DBTester

[![NuGet](https://img.shields.io/nuget/v/VulcanAnalytics.DBTester.svg)](https://www.nuget.org/packages/VulcanAnalytics.DBTester)

[![version][version-badge]][CHANGELOG]

[![license][license-badge]][LICENSE]

[![Join the chat at https://gitter.im/VulcanAnalytics-DBTester/Lobby](https://badges.gitter.im/VulcanAnalytics-DBTester/Lobby.svg)](https://gitter.im/VulcanAnalytics-DBTester/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

### What is DBTester?

DBTester is a testing framework library built to remove the need for database plumbing and repetitive code for performing utility actions from one test project to the next. Afterall why copy and paste the same database code from one project to the next, or worse still, write it out again?

This library is available through [NuGet](https://www.nuget.org/packages/VulcanAnalytics.DBTester) for immediate inclusion in your projects.

Further information to this README is available [on the wiki](https://github.com/VulcanAnalytics/DBTester/wiki)

### Ok, but what can I actually do with DBTester?

First, create an instance of DBTester with the MSSQL database you wish to interact with;

```csharp
using VulcanAnalytics.DBTester;
```

```csharp
var connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tempdb;Integrated Security=SSPI;";
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

This list of methods is short today, but increasing (as the database projects I'm currently working on requires further unit testing).
* [HasSchema](https://github.com/VulcanAnalytics/DBTester/wiki/HasSchema)
* [HasTable](https://github.com/VulcanAnalytics/DBTester/wiki/HasTable)
* [ClearTable](https://github.com/VulcanAnalytics/DBTester/wiki/ClearTable)
* [DropTable](https://github.com/VulcanAnalytics/DBTester/wiki/DropTable)
* [RowCount](https://github.com/VulcanAnalytics/DBTester/wiki/RowCount)
* [InsertData](https://github.com/VulcanAnalytics/DBTester/wiki/InsertData)
* [ExecuteStatementWithoutResult](https://github.com/VulcanAnalytics/DBTester/wiki/ExecuteStatementWithoutResult)
* [ExecuteStatementWithResult](https://github.com/VulcanAnalytics/DBTester/wiki/ExecuteStatementWithResult)

### Where did this come from?

This project has been born of the experience I have had in writing the same code for database unit and user acceptance testing in MSTest and SpecFlow. DBTester marks the third generation of this effort (both of the previous generations were client's closed source implementations).
In previous implementations there have been functionality for interacting with SQL Server Agent and SQL Server Integration Services revealing my background as a Business Intelligence and Data Warehousing Engineer.

### Now where is it going?

My roadmap for DBTester is to work towards a minimally functional yet stable release for version 1.0.0.

A few of the key aspects to have in place are;
- [x] high test coverage of the library
- [x] continuous integration and delivery
- [x] clear documentation on the wiki
- [x] NuGet packaging
- [ ] Abstract class to support extensions for other databases/techniques
- [ ] Basic implementation of another database as proof of concept

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

DBTester source code is Copyright &copy; 2018 Darren Comeau and other contributors under the [GNU GPLv3 License](LICENSE.txt).

If you have any questions regarding the license please [refer to this issue](https://github.com/VulcanAnalytics/DBTester/issues/11) and feel free to get in touch.

### My promise to you.

Here are a couple of notes about how this project will be maintained;
* Bugs will always come first, when you report a bug it will be the priority. Hopefully that will mean a rapid bug fix release, else advice on a work around and an estimate for which release the fix will be in.
* [Semantic Versioning](https://semver.org/) will be practiced religiously. You should always be confident that updating to the latest bug or minor release won't cause you an issue.
* Release notes will include clear details of the changes based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
* Major releases will not be so frequent as to force you to keep reworking your own projects.
* Major releases will also be supported with bug fixes and new minor features for as long as possible.

[CHANGELOG]: ./CHANGELOG.md
[LICENSE]: ./LICENSE

[version-badge]: https://img.shields.io/badge/version-0.10.0-blue.svg
[license-badge]: https://img.shields.io/badge/license-GPLv3-blue.svg
