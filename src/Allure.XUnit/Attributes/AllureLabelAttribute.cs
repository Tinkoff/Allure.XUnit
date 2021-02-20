using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AllureLabelAttribute : Attribute, IAllureInfo
    {
        public AllureLabelAttribute(string label, string value)
        {
            Label = label;
            Value = value;
        }

        public string Label { get; }
        public string Value { get; }
    }
}