# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- `ObjectData` method retrieves all rows and columns from a table or view [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/16)
### Changed
### Deprecated
### Removed
### Fixed
### Security

## [0.10.0-alpha] - 2018-10-13

### Added
- support net40 framework specific library in NuGet package [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/13)
- Logo for NuGet package [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/27)

### Removed
- net461 specific library from NuGet package [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/13)

## [0.9.0-alpha] - 2018-10 -12

### Changed
- `InsertData` method can insert defaulted columns into rows without additional data [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/20)

## [0.8.0-alpha] - 2018-10-11

### Added
- `ClearTable` method to remove all rows of data from a table [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/22)
- `DropTable` method to remove a table from the database if it exists [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/15)

## [0.7.2-alpha] - 2018-10-10

### Fixed
- `InsertData` method can insert null values into columns [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/19)

## [0.7.1-alpha] - 2018-10-09

### Fixed
- ~~`InsertData` method can insert null values into columns [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/19)~~

## [0.7.0-alpha] - 2018-10

### Added
- `ColumnDefaults` object which can be passed to `InsertData` and used as default values for unspecified columns [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/10)

### Fixed
- `InsertData` can't handle strings containing single quotes [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/17)

## [0.6.1-alpha] - 2018-10-07

### Fixed
- `System.Data` not referenced automatically by NuGet package [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/9)

## [0.6.0-alpha] - 2018-10-03

### Added
- `ExecuteStatementWithResult ` method added to retrieve data from an SQL statement [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/7)

### Changed
- Renamed `DatabaseTesterConnectionException` to `FailedDatabaseConnection`

## [0.5.1-alpha] - 2018-10-01

### Fixed
- Database connections don't timeout after two seconds [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/issues/5)

## [0.5.0-alpha] - 2018-09-29

### Added
- `MsSqlDatabaseTester` concrete implementation for `DatabaseTester`  [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/pull/3)

### Changed
- `DatabaseTester` now abstract class  [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/pull/3)

## [0.4.0-alpha] - 2018-09-28

### Added
- `InsertData` method added  [@darrencomeau](https://github.com/VulcanAnalytics/DBTester/pull/2)

## [0.3.0.1-alpha] - 2018-09

### Added
- README.md file

## [0.3.0-alpha] - 2018-09

## [0.2.0.1-alpha] - 2018-09

## [0.2.0-alpha] - 2018-09

## [0.1.0.7-alpha] - 2018-09
