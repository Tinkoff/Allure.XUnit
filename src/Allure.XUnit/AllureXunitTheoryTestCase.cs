using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Allure.Commons;
using Allure.Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Allure.XUnit
{
    internal class AllureXunitTheoryTestCase : XunitTheoryTestCase, ITestResultAccessor
    {
        private TestResult _testResult;
        public TestResultContainer TestResultContainer { get; set; }

        public TestResult TestResult
        {
            get => _testResult;
            set
            {
                Steps.Current = value;
                _testResult = value;
            }
        }

#pragma warning disable CS0618
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AllureXunitTheoryTestCase()
#pragma warning restore
        {
        }

        public AllureXunitTheoryTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay testMethodDisplay,
            TestMethodDisplayOptions defaultMethodDisplayOptions, ITestMethod testMethod)
            : base(diagnosticMessageSink, testMethodDisplay, defaultMethodDisplayOptions, testMethod)
        {
        }

        public override async Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            Steps.TestResultAccessor = this;
            messageBus = new AllureMessageBus(messageBus);
            var summary = await base.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator,
                cancellationTokenSource);
            return summary;
        }
    }
}