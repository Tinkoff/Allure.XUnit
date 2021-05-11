using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllureStoryAttribute : Attribute, IAllureInfo
    {
        public AllureStoryAttribute(params string[] story)
        {
            Stories = story;
        }

        internal string[] Stories { get; }
    }
}