using System;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

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
            _allureNUnitHelper.Value = new AllureNUnitHelper(test);
            
            if (test.IsSuite)
            {
                _allureNUnitHelper.Value.UpdateOneTimeFixture();
                _allureNUnitHelper.Value.StopFixture();
            }
            else
            {
                _allureNUnitHelper.Value.StartAll(_isWrappedIntoStep);
            }
        }

        public void AfterTest(ITest test)
        {
            if (test.IsSuite)
            {
                _allureNUnitHelper.Value ??= new AllureNUnitHelper(test);
                _allureNUnitHelper.Value.StopRootContainer();
            }
            else
            {
                _allureNUnitHelper.Value.StopAll(_isWrappedIntoStep);
            }
        }

        public ActionTargets Targets => ActionTargets.Test | ActionTargets.Suite;

        public void ApplyToContext(TestExecutionContext context)
        {
            _allureNUnitHelper.Value = new AllureNUnitHelper(context.CurrentTest);
            _allureNUnitHelper.Value.StartFixture();
        }
    }
}