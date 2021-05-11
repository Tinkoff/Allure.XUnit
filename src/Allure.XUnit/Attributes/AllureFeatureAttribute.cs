using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllureFeatureAttribute : Attribute, IAllureInfo
    {
        public AllureFeatureAttribute(params string[] feature)
        {
            Features = feature;
        }

        internal string[] Features { get; }
    }
}