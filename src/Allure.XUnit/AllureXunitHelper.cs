using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Allure.Commons;
using Allure.Xunit.Attributes;
using Xunit.Sdk;

namespace Allure.Xunit
{
    public class AllureXunitHelper
    {
        private readonly IXunitTestCase _test;

        private string _containerGuid;
        private static object ObjectLock => new object();
        private string _testResultGuid;
        public Dictionary<string, StatusDetails> StatusDetails { get; }

        static AllureXunitHelper()
        {
            const string allureConfigEnvVariable = "ALLURE_CONFIG";
            const string allureConfigName = "allureConfig.json";
            
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(allureConfigEnvVariable)))
                return;

            var allureConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                allureConfigName);
            
            Environment.SetEnvironmentVariable(allureConfigEnvVariable, allureConfigPath);
        }

        public AllureXunitHelper(IXunitTestCase test)
        {
            StatusDetails = new Dictionary<string, StatusDetails>();
            _test = test;
            var assemblyName = _test.TestMethod.TestClass.Class.Assembly.Name;

            if (_testAssembly != null)
                return;

            lock (ObjectLock)
            {
                if (_testAssembly == null) _testAssembly = Assembly.Load(assemblyName);
            }
        }

        private static Assembly _testAssembly;
        private static AllureLifecycle AllureLifecycle => AllureLifecycle.Instance;

        private void StartTestContainer()
        {
            const string  testContainerInterfix = "-tc-";
            _containerGuid = string.Concat(Guid.NewGuid().ToString(), testContainerInterfix , _test.UniqueID);
            var container = new TestResultContainer
            {
                uuid = _containerGuid,
                name = _test.DisplayName
            };
            AllureLifecycle.StartTestContainer(container);
        }

        private void StartTestCase()
        {
            const string  testResultInterfix = "-tc-";

            _testResultGuid = string.Concat(Guid.NewGuid().ToString(), testResultInterfix, _test.DisplayName);
            var testResult = new TestResult
            {
                uuid = _testResultGuid,
                name = _test.DisplayName,
                historyId = _test.DisplayName,
                fullName = _test.DisplayName,
                labels = new List<Label>
                {
                    Label.Thread(),
                    Label.Host(),
                    Label.TestClass(_test.TestMethod.TestClass.Class.Name),
                    Label.TestMethod(_test.DisplayName),
                    Label.Package(_test.TestMethod.TestClass.Class.Name)
                }
            };
            AllureLifecycle.StartTestCase(_containerGuid, testResult);
        }

        public void StopAll(RunSummary runSummary, ExceptionAggregator aggregator, bool isWrappedIntoStep = false)
        {
            StopTestCase(runSummary, aggregator);

            StopTestContainer();
        }

        private void StopTestCase(RunSummary runSummary, ExceptionAggregator aggregator)
        {
            UpdateTestDataFromAttributes();
            if (StatusDetails.ContainsKey(_test.UniqueID))
                AllureLifecycle.UpdateTestCase(x => x.statusDetails = StatusDetails[_test.UniqueID]);

            AllureLifecycle.StopTestCase(testCase => testCase.status = GeTestStatus(runSummary));
            AllureLifecycle.WriteTestCase(_testResultGuid);
        }

        private void StopTestContainer()
        {
            AllureLifecycle.StopTestContainer(_containerGuid);
            AllureLifecycle.WriteTestContainer(_containerGuid);
        }

        public void StartAll(bool isWrappedIntoStep)
        {
            StartTestContainer();
            StartTestCase();
        }

        private static Status GeTestStatus(RunSummary runSummary)
        {
            if (runSummary.Failed == 1)
                return Status.failed;
            if (runSummary.Skipped == 1)
                return Status.skipped;
            if (runSummary.Total == 1)
                return Status.passed;

            return Status.none;
        }

        private void UpdateTestDataFromAttributes()
        {
            var className = _test.TestMethod.TestClass.Class.Name;
            var methodName = _test.Method.Name;
            var attributes = _testAssembly.GetType(className).GetMethod(methodName)
                .GetCustomAttributes(typeof(IAllureInfo), true);

            foreach (var attribute in attributes)
                switch (attribute)
                {
                    case AllureFeatureAttribute featureAttr:
                        foreach (var feature in featureAttr.Features)
                            AllureLifecycle.UpdateTestCase(x => x.labels.Add(Label.Feature(feature)));
                        break;
                    case AllureLinkAttribute linkAttr:
                        AllureLifecycle.UpdateTestCase(x => x.links.Add(linkAttr.Link));
                        break;
                    case AllureIssueAttribute issueAttribute:
                        AllureLifecycle.UpdateTestCase(x => x.links.Add(issueAttribute.IssueLink));
                        break;
                    case AllureOwnerAttribute ownerAttribute:
                        AllureLifecycle.UpdateTestCase(x => x.labels.Add(Label.Owner(ownerAttribute.Owner)));
                        break;
                    case AllureSuiteAttribute suiteAttr:
                        AllureLifecycle.UpdateTestCase(x => x.labels.Add(Label.Suite(suiteAttr.Suite)));
                        break;
                    case AllureSubSuiteAttribute subSuiteAttr:
                        AllureLifecycle.UpdateTestCase(
                            x => x.labels.Add(Label.SubSuite(subSuiteAttr.SubSuite)));
                        break;
                    case AllureEpicAttribute epicAttr:
                        AllureLifecycle.UpdateTestCase(x => x.labels.Add(Label.Epic(epicAttr.Epic)));
                        break;
                    case AllureTagAttribute tagAttr:
                        foreach (var tag in tagAttr.Tags)
                            AllureLifecycle.UpdateTestCase(x => x.labels.Add(Label.Tag(tag)));
                        break;
                    case AllureSeverityAttribute severityAttr:
                        AllureLifecycle.UpdateTestCase(
                            x => x.labels.Add(Label.Severity(severityAttr.Severity)));
                        break;
                    case AllureParentSuiteAttribute parentSuiteAttr:
                        AllureLifecycle.UpdateTestCase(x =>
                            x.labels.Add(Label.ParentSuite(parentSuiteAttr.ParentSuite)));
                        break;
                    case AllureStoryAttribute storyAttr:
                        foreach (var story in storyAttr.Stories)
                            AllureLifecycle.UpdateTestCase(x => x.labels.Add(Label.Story(story)));
                        break;
                    case AllureDescriptionAttribute descriptionAttribute:
                        AllureLifecycle.UpdateTestCase(x => x.description = descriptionAttribute.Description);
                        break;
                    case AllureAddAttachmentAttribute allureAddAttachmentAttr:
                        break;
                }
        }
    }
}
