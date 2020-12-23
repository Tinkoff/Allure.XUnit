using Allure.Xunit;

namespace Examples
{
    public class AllureGetAttachmentPath : IAllureGetAttachmentPath
    {
        public string GetAttachmentPath()
        {
            return @"./allureConfig.json";
        }
    }
}
