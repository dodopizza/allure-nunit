using System;
using NUnit.Allure.Attributes;

namespace allure_nunit_tests
{
	public static class AllureStepsExamples
	{
		[AllureStep]
		public static void Step1()
		{
			Console.WriteLine("1");
		}

		[AllureStep("Step 3 - with explicit name")]
		public static void Step3()
		{
			Console.WriteLine("3");
		}

		[AllureStep("Step with params #{0} and #{1}")]
		public static void StepWithParams(object firstParam, object lastParam)
		{
			Console.WriteLine(firstParam);
			Console.WriteLine(lastParam);
		}
	}
}