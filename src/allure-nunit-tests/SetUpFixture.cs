using System;
using Allure.Commons;
using NUnit.Allure.Core;
using NUnit.Framework;

namespace allure_nunit_tests
{
	[SetUpFixture]
	public class SetUpFixture
	{
		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
			Console.WriteLine("CleanupResultDirectory");
			AllureExtensions.WrapSetUpTearDownParams(() => { AllureLifecycle.Instance.CleanupResultDirectory(); },
				"Clear Allure Results Directory");
		}
	}
}