using System;
using System.Threading.Tasks;
using Allure.Xunit;
using Allure.XUnit;
using Allure.Xunit.Attributes;
using Allure.Xunit.StepAttribute;
using Xunit;

namespace Examples
{
    public class ExampleSteps : IAsyncLifetime
    {
        [AllureBefore("Initialization")]
        public Task InitializeAsync()
        {
            Steps.Step("Nested", () => { });
            return Task.CompletedTask;
        }

        [AllureXunit]
        public async Task Test()
        {
            WriteHello(42, 4242, "secret");
            await AddAttachment();
        }

        [AllureStep("Write Hello")]
        private void WriteHello(int parameter, [Name("value")] int renameMe, [Skip] string password)
        {
            AllureMessageBus.TestOutputHelper.Value.WriteLine("Hello from Step");
        }

        [AllureStep("AddAttachment")]
        private async Task AddAttachment()
        {
            await AllureAttachments.Text("large json", "{}");
        }


        [AllureAfter("Cleanup")]
        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}