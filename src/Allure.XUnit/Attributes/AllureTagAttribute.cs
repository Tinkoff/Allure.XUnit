using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AllureTagAttribute : Attribute, IAllureInfo
    {
        public AllureTagAttribute(params string[] tags)
        {
            Tags = tags;
        }

        internal string[] Tags { get; }
    }
}