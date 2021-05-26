using System.Collections.Generic;
using Allure.XUnit;
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
            ITestMethod testMethod, IAttributeInfo thoeryAttribute)
        {
            var testCases = base.Discover(discoveryOptions, testMethod, thoeryAttribute);

            foreach (var item in testCases)
            {
                if (item.TestMethodArguments is not null)
                {
                    var testCase = new AllureXunitTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(),
                        TestMethodDisplayOptions.None, testMethod, item.TestMethodArguments);
                    yield return testCase;
                }
                else
                {
                    var testCase = new AllureXunitTheoryTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(),
                        TestMethodDisplayOptions.None, testMethod);
                    yield return testCase;
                }
            }
        }
    }
}