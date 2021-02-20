using System;
using MethodBoundaryAspect.Fody.Attributes;

namespace Allure.Xunit.StepAttribute
{
    /// <summary>
    ///     Decorates method as allure tear down using https://github.com/Fody/MethodDecorator
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
    public class AllureAfterAttribute : AllureStepAttributeBase
    {
        public AllureAfterAttribute(string name = null) : base(name)
        {
        }

        public override void OnEntry(MethodExecutionArgs arg)
        {
            Steps.StartAfterFixture(Name ?? arg.Method.Name);
            base.OnEntry(arg);
        }
    }
}