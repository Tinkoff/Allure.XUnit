using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllureStoryAttribute : AllureAttribute, IAllureInfo
    {
        public AllureStoryAttribute(string[] story, bool overwrite = false)
        {
            Stories = story;
            Overwrite = overwrite;
        }

        internal string[] Stories { get; }
    }
}