using System;
using Allure.Commons;

namespace NUnit.Allure.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class AllureIdAttribute : AllureTestCaseAttribute
{
    public AllureIdAttribute(string id)
    {
        Id = id;
    }

    private string Id { get; }

    public override void UpdateTestResult(TestResult testResult)
    {
        testResult.labels.Add(new Label{ name = "AS_ID", value = Id });
    }
}