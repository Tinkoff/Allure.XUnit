using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllureEpicAttribute : Attribute, IAllureInfo
    {
        public AllureEpicAttribute(string epic)
        {
            Epic = epic;
        }

        public string Epic { get; }
    }
}