using System;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllureOwnerAttribute : Attribute, IAllureInfo
    {
        public AllureOwnerAttribute(string owner)
        {
            Owner = owner;
        }

        internal string Owner { get; }
    }
}