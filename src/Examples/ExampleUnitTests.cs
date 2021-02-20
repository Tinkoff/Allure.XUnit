using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Allure.Commons;
using Allure.Xunit;
using Allure.XUnit;
using Allure.Xunit.Attributes;
using Xunit;

namespace Examples
{
    public class ExampleUnitTests : IDisposable
    {
        public void Dispose()
        {
        }

        public ExampleUnitTests()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
        }
    
        [AllureXunit]
        [AllureDescription("My test description")]
        [AllureParentSuite("AllTests")]
        [AllureFeature("qwerty", "123")]
        [AllureTag("TAG-1")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureIssue("ISSUE-1")]
        [AllureOwner("MyOwner")]
        [AllureSuite("PassedSuite")]
        [AllureSubSuite("NoAssert")]
        [AllureSubSuite("Simple")]
        [AllureLink("Google", "https://google.com")]
        [AllureEpic("TestEpic")]
        public void Test1()
        {
            Assert.True(1 != 1);
        }


        [AllureXunit]
        [AllureDescription("My test description2")]
        [AllureParentSuite("AllTests")]
        [AllureFeature("qwerty2", "1232")]
        [AllureTag("TAG-12")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureIssue("ISSUE-12")]
        [AllureOwner("MyOwner")]
        [AllureSuite("PassedSuite")]
        [AllureSubSuite("NoAssert2")]
        [AllureLink("Google", "https://google.com")]
        [AllureEpic("TestEpic")]
        public async Task Test2()
        {
            Assert.True(1 == 1);
            await AllureAttachments.File("allureConfig", @"./allureConfig.json");
        }

        [AllureXunit]
        [AllureDescription("My test description3")]
        [AllureParentSuite("AllTests")]
        [AllureFeature("qwerty3", "1232")]
        [AllureTag("TAG-12")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureIssue("ISSUE-12")]
        [AllureOwner("MyOwner")]
        [AllureSuite("PassedSuite")]
        [AllureSubSuite("NoAssert3")]
        [AllureLink("Google", "https://google.com")]
        [AllureEpic("TestEpic")]
        public void Test3()
        {
            Assert.Empty(new List<int>() {1, 2, 3});
        }
    }
}
