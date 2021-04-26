using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllureParentSuiteAttribute : Attribute, IAllureInfo
    {
        public AllureParentSuiteAttribute(string parentSuite)
        {
            ParentSuite = parentSuite;
        }

        internal string ParentSuite { get; }
    }
}