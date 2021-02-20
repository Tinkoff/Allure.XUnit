using System;

namespace Allure.Xunit.StepAttribute
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NameAttribute : Attribute
    {
        public NameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}