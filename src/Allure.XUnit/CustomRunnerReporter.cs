using Xunit;
using Xunit.Abstractions;

namespace Allure.Xunit
{
    public class CustomRunnerReporter : IRunnerReporter
    {
        public string Description => "My custom runner reporter";

        public bool IsEnvironmentallyEnabled => true;

        public string RunnerSwitch => "mycustomrunnerreporter";

        public IMessageSink CreateMessageHandler(IRunnerLogger logger)
        {
            return new CustomMessageSink();
        }
    }
}
