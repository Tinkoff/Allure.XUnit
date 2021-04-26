using Allure.Commons;

namespace Allure.XUnit
{
    public interface ITestResultAccessor
    {
        TestResultContainer TestResultContainer { get; set; }
        TestResult TestResult { get; set; }
    }
}