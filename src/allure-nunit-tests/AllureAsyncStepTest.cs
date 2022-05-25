using System.Threading.Tasks;
using Allure.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;

namespace allure_nunit_tests
{
	[AllureSuite("Tests - Async steps")]
	public class AllureAsyncStepTests : BaseTest
	{
		[Test]
		[AllureName("Simple test with async steps")]
		public async Task SimpleAsyncStepTest()
		{
			await AllureStepsAsyncExamples.AsyncStepWithAttribute();
			await AllureLifecycle.Instance.WrapInStepAsync(async () => await AllureStepsAsyncExamples.AsyncMethod("Step 2"));
			await AllureStepsAsyncExamples.AsyncStepWithAttribute();
		}
	}
}