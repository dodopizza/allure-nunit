using System;
using System.Linq;
using System.Text;
using System.Threading;
using Allure.Commons;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using TestResult = Allure.Commons.TestResult;

namespace NUnit.Allure.Core
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class AllureNUnitAttribute : PropertyAttribute, ITestAction, IApplyToContext
    {
        private readonly AsyncLocal<AllureNUnitHelper> _allureNUnitHelper = new AsyncLocal<AllureNUnitHelper>();
        private readonly bool _isWrappedIntoStep;

        public AllureNUnitAttribute(bool wrapIntoStep = true)
        {
            _isWrappedIntoStep = wrapIntoStep;
        }

        public void BeforeTest(ITest test)
        {
            if (test.IsSuite)
            {
                var currentResult = TestExecutionContext.CurrentContext.CurrentResult;
                
                AllureLifecycle.Instance.AddAttachment("Console Output", "text/plain",
                    Encoding.UTF8.GetBytes(currentResult.Output), ".txt");
                
                var testResult = new TestResult();
                AllureLifecycle.Instance.UpdateTestCase(fixtureResult =>
                {
                    fixtureResult.name = "OneTimeSetUp";
                    fixtureResult.status = testResult.steps.SelectMany(s => s.steps)
                        .All(s => s.status == Status.passed)
                        ? Status.passed
                        : Status.failed;
                    // testResult = fixtureResult;
                });

                // testResult.name = "OneTimeSetUp";
                // testResult.status = testResult.steps.SelectMany(s => s.steps)
                    // .All(s => s.status == Status.passed)
                    // ? Status.passed
                    // : Status.failed;

                // testFixture.Properties.Add("OneTimeSetUpResult", testResult);
            }
            else
            {
                _allureNUnitHelper.Value = new AllureNUnitHelper(test);
                _allureNUnitHelper.Value.StartAll(_isWrappedIntoStep);
            }
        }

        public void AfterTest(ITest test)
        {
            if (!test.IsSuite)
            {
                _allureNUnitHelper.Value.StopAll(_isWrappedIntoStep);
            }
        }

        public ActionTargets Targets => ActionTargets.Test | ActionTargets.Suite;

        public void ApplyToContext(TestExecutionContext context)
        {
            var helper = new AllureNUnitHelper(TestExecutionContext.CurrentContext.CurrentTest);
            
            helper.StartAll2(true);
        }
    }
}