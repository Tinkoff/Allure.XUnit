using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllureFeatureAttribute : AllureAttribute, IAllureInfo
    {
        public AllureFeatureAttribute(string[] feature, bool overwrite = false)
        {
            Features = feature;
            Overwrite = overwrite;
        }

        internal string[] Features { get; }
    }
}