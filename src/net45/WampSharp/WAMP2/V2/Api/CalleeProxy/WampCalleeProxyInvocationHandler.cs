using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.V2.Client;
using WampSharp.V2.Rpc;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract class WampCalleeProxyInvocationHandler : IWampCalleeProxyInvocationHandler
    {
        protected readonly Dictionary<string, object> mEmptyOptions = new Dictionary<string, object>();

        public object Invoke(MethodInfo method, object[] arguments)
        {
            Type unwrapped = TaskExtensions.UnwrapReturnType(method.ReturnType);

            // TODO: Add out/ref support. (It's too hard right now)
            Task<object> task = InnerInvokeAsync(method, arguments, unwrapped);

            try
            {
                object result = task.Result;
                return result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public Task InvokeAsync(MethodInfo method, object[] arguments)
        {
            Type returnType = method.ReturnType;

            Type unwrapped = TaskExtensions.UnwrapReturnType(returnType);

            Task<object> task = InnerInvokeAsync(method, arguments, unwrapped);

            Task casted = task.Cast(unwrapped);

            return casted;
        }

        private Task<object> InnerInvokeAsync(MethodInfo method, object[] arguments, Type returnType)
        {
            Callback<object> callback;

            if (HasMultivaluedResult(method))
            {
                callback = new MultiValueCallback(returnType);
            }
            else
            {
                callback = new SingleValueCallback(returnType);
            }

            WampProcedureAttribute procedureAttribute = 
                method.GetCustomAttribute<WampProcedureAttribute>(true);

            Invoke(arguments, callback, procedureAttribute.Procedure);

            return callback.Task;
        }

        protected abstract void Invoke(object[] arguments,
                                       IWampRawRpcOperationCallback callback,
                                       string procedure);

        private bool HasMultivaluedResult(MethodInfo method)
        {
            WampResultAttribute resultAttribute = 
                method.GetCustomAttribute<WampResultAttribute>(true);

            if (!method.ReturnType.IsArray)
            {
                return false;
            }

            if ((resultAttribute == null) ||
                (resultAttribute.CollectionResultTreatment == CollectionResultTreatment.Multivalued))
            {
                return true;
            }

            return false;
        }

        private abstract class Callback<T> : IWampRawRpcOperationCallback
        {
            private readonly TaskCompletionSource<T> mTask = new TaskCompletionSource<T>();

            public Task<T> Task
            {
                get { return mTask.Task; }
            }

            protected void SetResult(T result)
            {
                mTask.SetResult(result);
            }

            public abstract void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details);

            public abstract void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details,
                                                  TMessage[] arguments);

            public abstract void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details,
                                                  TMessage[] arguments,
                                                  TMessage argumentsKeywords);

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
            {
                mTask.SetException(ErrorExtractor.Error(formatter, details, error));
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error,
                                        TMessage[] arguments)
            {
                mTask.SetException(ErrorExtractor.Error(formatter, details, error, arguments));
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error,
                                        TMessage[] arguments,
                                        TMessage argumentsKeywords)
            {
                mTask.SetException(ErrorExtractor.Error(formatter, details, error, arguments, argumentsKeywords));
            }
        }

        private abstract class SimpleCallback : Callback<object>
        {
            protected abstract object GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments);

            public override void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details)
            {
                SetResult(null);
            }

            public override void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, TMessage[] arguments)
            {
                SetResult(formatter, arguments);
            }

            public override void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
            {
                SetResult(formatter, arguments);
            }

            private void SetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
            {
                object result = GetResult(formatter, arguments);
                SetResult(result);
            }
        }

        private class SingleValueCallback : SimpleCallback
        {
            private readonly Type mReturnType;

            public SingleValueCallback(Type returnType)
            {
                mReturnType = returnType;
            }

            protected override object GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
            {
                if (!arguments.Any())
                {
                    return null;
                }
                else
                {
                    object result = formatter.Deserialize(mReturnType, arguments[0]);
                    return result;
                }
            }
        }

        private class MultiValueCallback : SimpleCallback
        {
            private readonly Type mReturnType;
            private readonly Type mElementType;

            public MultiValueCallback(Type returnType)
            {
                mReturnType = returnType;

                mElementType = returnType.GetElementType();
            }

            protected override object GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
            {
                if (!arguments.Any())
                {
                    return null;
                }
                else
                {
                    object[] deserialized =
                        arguments.Select(x => formatter.Deserialize(mElementType, x))
                                 .ToArray();

                    Array array = 
                        Array.CreateInstance(mElementType, deserialized.Length);

                    deserialized.CopyTo(array, 0);

                    return array;
                }
            }
        }
    }
}