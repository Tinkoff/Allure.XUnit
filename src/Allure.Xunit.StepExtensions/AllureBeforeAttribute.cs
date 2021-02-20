using System;
using MethodBoundaryAspect.Fody.Attributes;

namespace Allure.Xunit.StepAttribute
{
    /// <summary>
    ///     Decorates method as allure tear up using https://github.com/Fody/MethodDecorator
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
    public class AllureBeforeAttribute : AllureStepAttributeBase
    {
        public AllureBeforeAttribute(string name = null) : base(name)
        {
        }

        public override void OnEntry(MethodExecutionArgs arg)
        {
            Steps.StartBeforeFixture(Name ?? arg.Method.Name);
            base.OnEntry(arg);
        }
    }
}