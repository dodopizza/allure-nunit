using System;
using System.Threading.Tasks;
using Allure.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;

namespace allure_nunit_tests
{
	public static class AllureStepsAsyncExamples
	{
		private static readonly Random Random = new();
		
		[AllureStep]
		public static async Task AsyncStepWithAttribute()
		{
			await AsyncMethod($"Step {nameof(AsyncStepWithAttribute)}");
		}
		
		public static async Task AsyncWrappedStep()
		{
			await AllureLifecycle.Instance.WrapInStepAsync(async () => await AsyncMethod($"Step {nameof(AsyncWrappedStep)}"));
		}
		
		public static async Task<string> AsyncMethod(string message) => await Task.Run(() =>
		{
			for (var i = 0; i < 3; i++)
			{
				var delay = Random.Next(1, 100);
				Console.WriteLine($"Attempt {i} \"{message}\" delay {delay}");
				Task.Delay(delay).Wait();
			}

			return string.Empty;
		});
	}
}