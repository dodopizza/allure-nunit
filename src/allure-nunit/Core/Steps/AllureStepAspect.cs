﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using Allure.Commons;
using AspectInjector.Broker;
using NUnit.Allure.Attributes;

namespace NUnit.Allure.Core.Steps
{
    [Aspect(Scope.Global)]
    public class AllureStepAspect
    {
        private static readonly MethodInfo AsyncErrorHandler = typeof(AllureStepAspect).GetMethod(nameof(AllureStepAspect.WrapAsync), BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo AsyncGenericErrorHandler = typeof(AllureStepAspect).GetMethod(nameof(AllureStepAspect.WrapAsyncGeneric), BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo SyncGenericErrorHandler = typeof(AllureStepAspect).GetMethod(nameof(AllureStepAspect.WrapSyncGeneric), BindingFlags.NonPublic | BindingFlags.Static);
        
        [Advice(Kind.Around, Targets = Target.Method)]
        public object WrapStep(
            [Argument(Source.Name)] string name,
            [Argument(Source.Metadata)] MethodBase methodBase,
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Target)] Func<object[], object> method,
            [Argument(Source.ReturnType)] Type retType)
        {
            var stepName = methodBase.GetCustomAttribute<AllureStepAttribute>().StepName;

            for (var i = 0; i < arguments.Length; i++)
            {
              
                stepName = stepName?.Replace("{" + i + "}", arguments[i]?.ToString() ?? "null");
            }

            var stepResult = string.IsNullOrEmpty(stepName)
                ? new StepResult {name = name, parameters = AllureStepParameterHelper.CreateParameters(arguments)}
                : new StepResult {name = stepName, parameters = AllureStepParameterHelper.CreateParameters(arguments)};

            object result = null;
            try
            {
                AllureLifecycle.Instance.StartStep(Guid.NewGuid().ToString(), stepResult);
                if (typeof(Task).IsAssignableFrom(retType))
                {
                    if (retType == typeof(Task))
                        result = AsyncErrorHandler.Invoke(this, new object[] { method, arguments });
                    else
                    {
                        var syncResultType = retType.IsConstructedGenericType ? retType.GenericTypeArguments[0] : typeof(object);
                        result =  AsyncGenericErrorHandler.MakeGenericMethod(syncResultType).Invoke(this, new object[] { method, arguments });
                    }
                }
                else
                {
                    if (retType == typeof(void))
                        result = method(arguments);
                    else
                        result = SyncGenericErrorHandler.MakeGenericMethod(retType)
                            .Invoke(this, new object[] { method, arguments });
                }

                AllureLifecycle.Instance.StopStep(step => stepResult.status = Status.passed);
            }
            catch (Exception e)
            {
                AllureLifecycle.Instance.StopStep(step =>
                {
                    step.statusDetails = new StatusDetails
                    {
                        message = e.Message,
                        trace = e.StackTrace
                    };
                    step.status = Status.failed;
                });
                throw;
            }

            return result;
        }
        
        private static T WrapSync<T>(Func<object[], object> target, object[] args)
        {
            return (T)target(args);
        }

        private static async Task WrapAsync(Func<object[], object> target, object[] args)
        {
            await (Task)target(args);
        }

        private static async Task<T> WrapAsyncGeneric<T>(Func<object[], object> target, object[] args)
        {
            return await (Task<T>)target(args);
        }
        
        private static T WrapSyncGeneric<T>(Func<object[], object> target, object[] args)
        {
            return (T)target(args);
        }
    }
    

}