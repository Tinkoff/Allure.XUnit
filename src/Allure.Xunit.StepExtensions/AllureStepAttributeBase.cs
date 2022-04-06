using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Allure.Commons;
using MethodBoundaryAspect.Fody.Attributes;

namespace Allure.Xunit.StepAttribute
{
    public abstract class AllureStepAttributeBase : OnMethodBoundaryAspect
    {
        private static readonly ConcurrentDictionary<Type, Action<MethodExecutionArgs>> ContinuationsCache = new();
        protected readonly string Name;

        protected AllureStepAttributeBase(string name = null)
        {
            Name = name;
        }

        public override void OnEntry(MethodExecutionArgs arg)
        {
            if (arg.Arguments.Any())
            {
                Steps.Current.parameters = arg.Method.GetParameters()
                    .Select(x => (
                        name: x.GetCustomAttribute<NameAttribute>()?.Name ?? x.Name,
                        skip: x.GetCustomAttribute<SkipAttribute>() != null))
                    .Zip(arg.Arguments, (attr, value) => attr.skip
                        ? null
                        : new Parameter
                        {
                            name = attr.name,
                            value = value?.ToString()
                        })
                    .Where(x => x != null)
                    .ToList();
            }

            arg.MethodExecutionTag = Steps.Current;
        }

        public override void OnExit(MethodExecutionArgs arg)
        {
            if (arg.ReturnValue == null)
            {
                Steps.PassStep((ExecutableItem) arg.MethodExecutionTag);
                return;
            }

            static Type GetTaskType(Type type)
            {
                while (true)
                {
                    if (type == null)
                    {
                        return null;
                    }

                    if (type == typeof(Task) ||
                        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>) ||
                        type == typeof(ValueTask) ||
                        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValueTask<>))
                    {
                        return type;
                    }
                    type = type.BaseType;
                }
            }

            var taskType = GetTaskType(arg.ReturnValue.GetType());
            if (taskType != null)
            {
                var action = ContinuationsCache.GetOrAdd(taskType, type =>
                {
                    MethodInfo methodInfo;
                    if (type == typeof(Task))
                    {
                        methodInfo = typeof(AllureStepAttributeBase)
                            .GetMethod(nameof(ContinueTask));
                    }
                    else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        var resultType = type.GetGenericArguments().FirstOrDefault();
                        methodInfo = typeof(AllureStepAttributeBase)
                            .GetMethod(nameof(ContinueTaskGeneric))
                            .MakeGenericMethod(resultType);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }

                    return (Action<MethodExecutionArgs>) methodInfo.CreateDelegate(typeof(Action<MethodExecutionArgs>));
                });
                action.Invoke(arg);
            }
            else
            {
                Steps.PassStep((ExecutableItem) arg.MethodExecutionTag);
            }
        }

        public override void OnException(MethodExecutionArgs arg) =>
            Steps.FailStep((ExecutableItem) arg.MethodExecutionTag);

        public static void ContinueTaskGeneric<T>(MethodExecutionArgs args)
        {
            var returnValue = (Task<T>) args.ReturnValue;
            returnValue.ContinueWith(task =>
            {
                Steps.PassStep((ExecutableItem) args.MethodExecutionTag);
                return task.Result;
            }, TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
        }

        public static void ContinueTask(MethodExecutionArgs args)
        {
            var returnValue = (Task) args.ReturnValue;
            returnValue.ContinueWith(_ => Steps.PassStep((ExecutableItem) args.MethodExecutionTag),
                TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);
        }
    }
}