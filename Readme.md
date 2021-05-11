# Allure.XUnit

[![Build Status](https://cloud.drone.io/api/badges/TinkoffCreditSystems/Allure.XUnit/status.svg)](https://cloud.drone.io/TinkoffCreditSystems/Allure.XUnit)
[![Nuget](https://img.shields.io/nuget/v/Allure.XUnit)](https://www.nuget.org/packages/Allure.XUnit/)

Allure.XUnit is library for display xunit tests in Allure report.

Allure.XUnit supports .NET Core 2.0 and later.

## Attributes:

* AllureXunit
* AllureDescription
* AllureParentSuite
* AllureFeature
* AllureTag
* AllureSeverity
* AllureIssue
* AllureOwner
* AllureSuite
* AllureSubSuite
* AllureLink
* AllureEpic
* AllureLabel
* AllureXunitTheory - attribute for display parametrized tests. Use ```InlineData```, ```MemberData```, ```ClassData```,
  XUnit attributes for pass parameters.

All methods have to be tagged by attribute AllureXunit instead of Fact, or AllureXunitTheory instead of Theory for
display in allure report. Other attributes are optional.

## Steps
There are two ways to describe steps:
1. Use [`Steps`](src/Allure.XUnit/Steps.cs) class for functional or imperative approach.
2. Use `AllureStepAttribute`, `AllureBeforeAttribute`, `AllureAfterAttribute` for declarative approach.

See [Examples](src/Allure.Xunit.StepExtensions.Examples/ExampleSteps.cs).

## Attachments
Use [`AllureAttachments`](src/Allure.XUnit/AllureAttachments.cs) class with it's methods.

## Running

Just run `dotnet test`.

`allure-results` directory with result appears after running tests in target directory.

Execute command ```allure serve ./ ``` in allure-results directory to local viewing tests. Also you can browse result
using similar [docker-compose file](./src/Allure.XUnit.Examples/docker-compose.yaml).

## Examples

See [Examples](src/Allure.XUnit.Examples)

## Author
[Shumakov Ivan](https://github.com/IvanWR1995)
