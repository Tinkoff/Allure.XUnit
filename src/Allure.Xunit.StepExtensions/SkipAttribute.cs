using System;

namespace Allure.Xunit.StepAttribute
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SkipAttribute : Attribute
    {
    }
}