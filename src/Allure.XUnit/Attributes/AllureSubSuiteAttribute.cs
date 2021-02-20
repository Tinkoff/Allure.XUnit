using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AllureSubSuiteAttribute : Attribute, IAllureInfo
    {
        public AllureSubSuiteAttribute(string subSuite)
        {
            SubSuite = subSuite;
        }

        internal string SubSuite { get; }
    }
}