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
* AllureXunitTheory - attribute for display parametrized tests. Use ```InlineData```, ```MemberData```, ```ClassData```, XUnit attributes for pass parameters.
* AllureAddAttachment - attribute for attach files to tests. AllureAddAttachment take as paramet file path or Type of object witch implement   IAllureGetAttachmentPath interface for calculate attachment file path.
```
IAllureGetAttachmentPath
{
       string GetAttachmentPath(); //calculate attachment file path in runtime
}
```
 All methods have to be tagged by attribute AllureXunit inteasd of Fact, or AllureXunitTheory instead of Theory for display in allure report. Other attributes are optional.

 ## Running
 For start use: dotnet test.
 
 allure-results directory with result appears after running tests in target dirrectory 

 Execute command ```allure serve ./ ``` in allure-results directory to local viewing tests.

## Examples
[Examples](src/Examples)

 ## Author
 [Shumakov Ivan](https://github.com/IvanWR1995)
