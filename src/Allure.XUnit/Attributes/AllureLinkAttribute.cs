using System;
using Allure.Commons;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AllureLinkAttribute : Attribute, IAllureInfo
    {
        public AllureLinkAttribute(string name, string url)

        {
            Link = new()
            {
                name = name,
                type = "link",
                url = url
            };
        }

        public AllureLinkAttribute(string url)
        {
            Link = new()
            {
                name = url,
                type = "link",
                url = url
            };
        }

        internal Link Link { get; }
    }
}