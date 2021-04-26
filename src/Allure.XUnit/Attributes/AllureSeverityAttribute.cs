using System;
using Allure.Commons;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllureSeverityAttribute : Attribute, IAllureInfo
    {
        public AllureSeverityAttribute(SeverityLevel severity = SeverityLevel.normal)
        {
            Severity = severity;
        }

        internal SeverityLevel Severity { get; }
    }
}