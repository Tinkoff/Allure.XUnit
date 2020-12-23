using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Allure.Xunit
{
    public class AllureXunitTheoryDiscover : TheoryDiscoverer
    {
        public AllureXunitTheoryDiscover(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
        }

        public override IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            var testCases = base.Discover(discoveryOptions, testMethod, factAttribute);

            foreach (var item in testCases)
            {
                var testCase = new AllureXunitTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(),
                    TestMethodDisplayOptions.None, testMethod, item.TestMethodArguments);
                CustomMessageSink.AddTestCaseHandling(testCase);
                yield return testCase;
            }
        }
    }
}
