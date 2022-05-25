using System;
using System.Threading.Tasks;
using Allure.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;

namespace allure_nunit_tests
{
	[AllureSuite("Tests: Async OneTime SetUp")]
	[AllureNUnit]
	[Parallelizable(ParallelScope.All)]
	public class AllureAsyncOneTimeSetUpTest
	{
		[OneTimeSetUp]
		public async Task OneTimeSetUp1()
		{
			await AllureStepsAsyncExamples.AsyncStepWithAttribute();
			await AllureStepsAsyncExamples.AsyncWrappedStep1();
			await AllureStepsAsyncExamples.AsyncWrappedStep2();
		}

		[SetUp]
		public void SetUp1()
		{
			AllureStepsExamples.Step1();
		}
		
		[Test]
		[AllureName("Test")]
		public async Task Test()
		{
			await AllureStepsAsyncExamples.AsyncWrappedStep();
		}
		
		[Test]
		[AllureName("Test2")]
		public async Task Test2()
		{
			await AllureStepsAsyncExamples.AsyncWrappedStep();
		}
	}
}