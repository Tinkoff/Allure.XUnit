using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Allure.Commons;
using Allure.XUnit;
using Allure.Xunit.Attributes;
using Xunit.Abstractions;

namespace Allure.Xunit
{
    public static class AllureXunitHelper
    {
        static AllureXunitHelper()
        {
            const string allureConfigEnvVariable = "ALLURE_CONFIG";
            const string allureConfigName = "allureConfig.json";

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(allureConfigEnvVariable)))
            {
                return;
            }

            var allureConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, allureConfigName);

            Environment.SetEnvironmentVariable(allureConfigEnvVariable, allureConfigPath);
        }

        public static void StartTestCase(ITestCaseStarting testCaseStarting)
        {
            if (testCaseStarting.TestCase is not ITestResultAccessor testResults)
            {
                return;
            }

            StartTestContainer(testCaseStarting.TestClass, testResults);
            var testCase = testCaseStarting.TestCase;
            var uuid = NewUuid(testCase.DisplayName);
            testResults.TestResult = new()
            {
                uuid = uuid,
                name = testCase.DisplayName,
                historyId = testCase.DisplayName,
                fullName = testCase.DisplayName,
                labels = new()
                {
                    Label.Thread(),
                    Label.Host(),
                    Label.TestClass(testCase.TestMethod.TestClass.Class.Name),
                    Label.TestMethod(testCase.DisplayName),
                    Label.Package(testCase.TestMethod.TestClass.Class.Name)
                }
            };
            UpdateTestDataFromAttributes(testResults.TestResult, testCase);
            AllureLifecycle.Instance.StartTestCase(testResults.TestResultContainer.uuid, testResults.TestResult);
        }

        public static void MarkTestCaseAsFailed(ITestFailed testFailed)
        {
            if (testFailed.TestCase is not ITestResultAccessor testResults)
            {
                return;
            }

            var statusDetails = testResults.TestResult.statusDetails ??= new();
            statusDetails.trace = string.Join('\n', testFailed.StackTraces);
            statusDetails.message = string.Join('\n', testFailed.Messages);
            testResults.TestResult.status = Status.failed;
        }

        public static void MarkTestCaseAsPassed(ITestPassed testPassed)
        {
            if (testPassed.TestCase is not ITestResultAccessor testResults)
            {
                return;
            }

            var statusDetails = testResults.TestResult.statusDetails ??= new();
            statusDetails.message = testPassed.Output;
            testResults.TestResult.status = Status.passed;
        }

        public static void FinishTestCase(ITestCaseFinished testCaseFinished)
        {
            if (testCaseFinished.TestCase is not ITestResultAccessor testResults)
            {
                return;
            }

            AllureLifecycle.Instance.StopTestCase(testResults.TestResult.uuid);
            AllureLifecycle.Instance.WriteTestCase(testResults.TestResult.uuid);
            AllureLifecycle.Instance.StopTestContainer(testResults.TestResultContainer.uuid);
            AllureLifecycle.Instance.WriteTestContainer(testResults.TestResultContainer.uuid);
        }

        private static void StartTestContainer(ITestClass testClass, ITestResultAccessor testResult)
        {
            var uuid = NewUuid(testClass.Class.Name);
            testResult.TestResultContainer = new()
            {
                uuid = uuid,
                name = testClass.Class.Name
            };
            AllureLifecycle.Instance.StartTestContainer(testResult.TestResultContainer);
        }

        private static string NewUuid(string name)
        {
            var uuid = string.Concat(Guid.NewGuid().ToString(), "-", name);
            return uuid;
        }

        internal static void Log(string message)
        {
            AllureMessageBus.TestOutputHelper.Value.WriteLine("╬════════════════════════");
            AllureMessageBus.TestOutputHelper.Value.WriteLine($"║ {message}");
            AllureMessageBus.TestOutputHelper.Value.WriteLine("╬═══════════════");
        }

        private static void AddDistinct(this List<Label> labels, Label labelToInsert)
        {
            var existingLabel = labels.FirstOrDefault(label => label.name.Equals(labelToInsert.name));
            if (existingLabel != null)
            {
                existingLabel.value = labelToInsert.value;
            }
            else
            {
                labels.Add(labelToInsert);
            }
        }

        private static void UpdateTestDataFromAttributes(TestResult testResult, ITestCase testCase)
        {
            var classAttributes = testCase.TestMethod.TestClass.Class.GetCustomAttributes(typeof(IAllureInfo));
            var methodAttributes = testCase.TestMethod.Method.GetCustomAttributes(typeof(IAllureInfo));

            foreach (var attribute in classAttributes.Concat(methodAttributes))
            {
                switch (((IReflectionAttributeInfo) attribute).Attribute)
                {
                    case AllureFeatureAttribute featureAttribute:
                        foreach (var feature in featureAttribute.Features)
                        {
                            // Features can be added multiple times without overwriting
                            testResult.labels.Add(Label.Feature(feature));
                        }

                        break;

                    case AllureLinkAttribute linkAttribute:
                        testResult.links.Add(linkAttribute.Link);
                        break;

                    case AllureIssueAttribute issueAttribute:
                        testResult.links.Add(issueAttribute.IssueLink);
                        break;

                    case AllureOwnerAttribute ownerAttribute:
                        testResult.labels.AddDistinct(Label.Owner(ownerAttribute.Owner));
                        break;

                    case AllureSuiteAttribute suiteAttribute:
                        testResult.labels.AddDistinct(Label.Suite(suiteAttribute.Suite));
                        break;

                    case AllureSubSuiteAttribute subSuiteAttribute:
                        testResult.labels.AddDistinct(Label.SubSuite(subSuiteAttribute.SubSuite));
                        break;

                    case AllureEpicAttribute epicAttribute:
                        testResult.labels.AddDistinct(Label.Epic(epicAttribute.Epic));
                        break;

                    case AllureTagAttribute tagAttribute:
                        foreach (var tag in tagAttribute.Tags)
                        {
                            // Tags can be added multiple times without overwriting
                            testResult.labels.Add(Label.Tag(tag));
                        }

                        break;

                    case AllureSeverityAttribute severityAttribute:
                        testResult.labels.AddDistinct(Label.Severity(severityAttribute.Severity));
                        break;

                    case AllureParentSuiteAttribute parentSuiteAttribute:
                        testResult.labels.AddDistinct(Label.ParentSuite(parentSuiteAttribute.ParentSuite));
                        break;

                    case AllureStoryAttribute storyAttribute:
                        foreach (var story in storyAttribute.Stories)
                        {
                            // Stories can be added multiple times without overwriting
                            testResult.labels.Add(Label.Story(story));
                        }

                        break;

                    case AllureDescriptionAttribute descriptionAttribute:
                        testResult.description = descriptionAttribute.Description;
                        break;

                    case AllureLabelAttribute labelAttribute:
                        testResult.labels.AddDistinct(new()
                        {
                            name = labelAttribute.Label,
                            value = labelAttribute.Value
                        });
                        break;
                }
            }
        }
    }
}