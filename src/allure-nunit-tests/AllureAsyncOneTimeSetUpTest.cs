using System.Threading.Tasks;
using NUnit.Allure.Attributes;
using NUnit.Framework;

namespace allure_nunit_tests
{
	[AllureSuite("Tests: Async OneTime SetUp")]
	[Parallelizable(ParallelScope.All)]
	public class AllureAsyncOneTimeSetUpTest: BaseTest
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
		[AllureName("Test1")]
		public async Task Test1()
		{
			await AllureStepsAsyncExamples.AsyncWrappedStep();
			await AllureStepsAsyncExamples.AsyncStepWithAttribute();
		}

		[Test]
		[AllureName("Test2")]
		public async Task Test2()
		{
			await AllureStepsAsyncExamples.AsyncStepWithAttribute();
			await AllureStepsAsyncExamples.AsyncWrappedStep();
		}
		
		[Test]
		[AllureName("Test3")]
		public async Task Test3()
		{
			await AllureStepsAsyncExamples.AsyncStepWithAttribute();
			await AllureStepsAsyncExamples.AsyncWrappedStep();
			await AllureStepsAsyncExamples.AsyncStepWithAttribute();
			await AllureStepsAsyncExamples.AsyncWrappedStep();
		}
	}
}