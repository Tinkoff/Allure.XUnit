using System.Collections.Generic;
using Xunit.Abstractions;

namespace Allure.Xunit
{
    public class CustomMessageSink : IMessageSink
    {
        private static readonly Dictionary<string, AllureXunitTestCase> TestCasesDictionary =
            new Dictionary<string, AllureXunitTestCase>();

        public bool OnMessage(IMessageSinkMessage message)
        {
            switch (message)
            {
                case ITestCaseMessage testCaseMessage:
                    var uniqueId = testCaseMessage.TestCase.UniqueID;
                    TestCasesDictionary.GetValueOrDefault(uniqueId)?.OnMessage(message);
                    break;
                case ITestAssemblyFinished testAssemblyFinished:
                    foreach (var testCase in TestCasesDictionary.Values)
                        testCase.Stop();
                    break;
            }

            return true;
        }

        public static void AddTestCaseHandling(AllureXunitTestCase testCase)
        {
            TestCasesDictionary.Add(testCase.UniqueID, testCase);
        }
    }
}
