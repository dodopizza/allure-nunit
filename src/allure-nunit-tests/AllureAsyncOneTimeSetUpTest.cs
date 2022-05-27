using System.Threading.Tasks;
using NUnit.Allure.Attributes;
using NUnit.Framework;

namespace allure_nunit_tests
{
	[AllureSuite("Tests - Async OneTime SetUp")]
	[Parallelizable(ParallelScope.All)]
	public class AllureAsyncOneTimeSetUpTest: BaseTest
	{
		[OneTimeSetUp]
		public async Task OneTimeSetUp()
		{
			await AllureStepsAsyncExamples.AsyncStepWithAttribute();
			await AllureStepsAsyncExamples.AsyncWrappedStep();
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
		public void Test2()
		{
			AllureStepsExamples.Step1();
		}
	}
}