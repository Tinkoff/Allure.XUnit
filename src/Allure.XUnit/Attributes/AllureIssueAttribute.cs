using System;
using Allure.Commons;

namespace Allure.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllureIssueAttribute : Attribute, IAllureInfo
    {
        public AllureIssueAttribute(string name, string url)
        {
            IssueLink = new()
            {
                name = name,
                type = "issue",
                url = url
            };
        }

        public AllureIssueAttribute(string name)
        {
            IssueLink = new()
            {
                name = name,
                type = "issue",
                url = name
            };
        }

        internal Link IssueLink { get; }
    }
}