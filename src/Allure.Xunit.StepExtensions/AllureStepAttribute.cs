using System;
using MethodBoundaryAspect.Fody.Attributes;

namespace Allure.Xunit.StepAttribute
{
    /// <summary>
    ///     Decorates method as allure step using https://github.com/Fody/MethodDecorator
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
    public class AllureStepAttribute : AllureStepAttributeBase
    {
        public AllureStepAttribute(string name = null) : base(name)
        {
        }

        public override void OnEntry(MethodExecutionArgs arg)
        {
            Steps.StartStep(Name ?? arg.Method.Name);
            base.OnEntry(arg);
        }
    }
}