using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Allure.Commons;
using Xunit.Abstractions;
using Xunit.Sdk;
using TestMethodDisplay = Xunit.Sdk.TestMethodDisplay;
using TestMethodDisplayOptions = Xunit.Sdk.TestMethodDisplayOptions;

namespace Allure.Xunit
{
    public class AllureXunitTestCase : XunitTestCase
    {
        private AllureXunitHelper _allureXunitHelper;
        private RunSummary _runSummary;
        private ExceptionAggregator _aggregateException;

        private ITestResultMessage TestResultMessage { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public AllureXunitTestCase()
        {
        }

        public AllureXunitTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay testMethodDisplay,
            TestMethodDisplayOptions defaultMethodDisplayOptions,
            ITestMethod testMethod, object[] testMethodArguments = null)
            : base(diagnosticMessageSink, testMethodDisplay, defaultMethodDisplayOptions, testMethod,
                testMethodArguments)
        {
            _allureXunitHelper = new AllureXunitHelper(this);
        }

        public void OnMessage(IMessageSinkMessage message)
        {
            switch (message)
            {
                case ITestPassed testPassed:
                case ITestFailed testFailed:
                    TestResultMessage = (TestResultMessage) message;
                    return;
                case ITestCaseFinished testFinished:
                    break;

                default:
                    return;
            }
        }

        public void Stop()
        {
            switch (TestResultMessage)
            {
                case ITestPassed testPassed
                    when (testPassed?.TestCase.UniqueID == UniqueID):
                    _allureXunitHelper.StartAll(true);
                    _allureXunitHelper.StatusDetails.Add(UniqueID,
                        new StatusDetails()
                        {
                            message = testPassed.Output
                        });
                    break;
                case ITestFailed testFailed
                    when (testFailed?.TestCase.UniqueID == UniqueID):
                    _allureXunitHelper.StartAll(true);
                    _allureXunitHelper.StatusDetails.Add(UniqueID,
                        new StatusDetails()
                        {
                            trace = string.Join(Environment.NewLine, testFailed.StackTraces),
                            message = string.Join(Environment.NewLine, testFailed.Messages)
                        });
                    break;
                default:
                    return;
            }

            _allureXunitHelper.StopAll(_runSummary, _aggregateException);
        }

        public override async Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            var summary = await base.RunAsync(diagnosticMessageSink, messageBus, constructorArguments,
                aggregator, cancellationTokenSource);
            
            _runSummary = summary;
            _aggregateException = aggregator;

            return summary;
        }
    }
}
