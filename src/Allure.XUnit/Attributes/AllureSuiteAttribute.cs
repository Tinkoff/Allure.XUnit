using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllureSuiteAttribute : Attribute, IAllureInfo
    {
        public AllureSuiteAttribute(string suite)
        {
            Suite = suite;
        }

        internal string Suite { get; }
    }
}