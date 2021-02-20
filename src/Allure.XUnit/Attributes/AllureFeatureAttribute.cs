using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllureFeatureAttribute : Attribute, IAllureInfo
    {
        public AllureFeatureAttribute(params string[] feature)
        {
            Features = feature;
        }

        internal string[] Features { get; }
    }
}