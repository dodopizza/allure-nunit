using System;
using Allure.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;

namespace allure_nunit_tests
{
    [AllureSuite("Tests - Steps")]
    public class AllureStepTest : BaseTest
    {
        [Test]
        [AllureName("Simple test with steps")]
        public void SimpleStepTest()
        {
            AllureStepsExamples.Step1();
            AllureLifecycle.Instance.WrapInStep(() => { Console.WriteLine("Step 2"); }, "Step2");
            AllureStepsExamples.Step3();
        }

        [Test]
        [AllureName("Complex test with steps")]
        public void WrappedStepTest()
        {
            AllureLifecycle.Instance.WrapInStep(() =>
            {
                AllureStepsExamples.Step1();

                AllureLifecycle.Instance.WrapInStep(() =>
                {
                    Console.WriteLine("2");
                    AllureLifecycle.Instance.WrapInStep(() => { Console.WriteLine("Step in step 2"); },
                        "Step in Step 2");
                }, "Step2");

                AllureStepsExamples.Step3();
            }, "RootStep");
        }

       

        [Test]
        [AllureName("Test with parametrized steps")]
        public void SimpleStepTest2()
        {
            AllureStepsExamples.StepWithParams("0", "1");
            AllureStepsExamples.StepWithParams(1, 2);
            AllureStepsExamples.StepWithParams(new[] { 1, 3, 5}, "array");
        }
    }
}