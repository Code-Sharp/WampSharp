using System;
using System.Linq;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract partial class WampCalleeProxyInvocationHandler
    {
        private abstract class AsyncOperationCallback : IWampRawRpcOperationCallback
        {
            private readonly TaskCompletionSource<object> mTask = new TaskCompletionSource<object>();

            public Task<object> Task
            {
                get { return mTask.Task; }
            }

            protected void SetResult(object result)
            {
                mTask.SetResult(result);
            }

            protected abstract object GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments);

            public void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details)
            {
                SetResult(null);
            }

            public void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, TMessage[] arguments)
            {
                SetResult(formatter, arguments);
            }

            public void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
            {
                SetResult(formatter, arguments);
            }

            private void SetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
            {
                object result = GetResult(formatter, arguments);
                SetResult(result);
            }

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


        private class SingleValueAsyncOperationCallback : AsyncOperationCallback
        {
            private readonly Type mReturnType;
            private readonly bool mHasReturnValue;

            public SingleValueAsyncOperationCallback(Type returnType, bool hasReturnValue)
            {
                mReturnType = returnType;
                mHasReturnValue = hasReturnValue;
            }

            protected override object GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
            {
                if (!mHasReturnValue || !arguments.Any())
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

        private class MultiValueAsyncOperationCallback : AsyncOperationCallback
        {
            private readonly Type mReturnType;
            private readonly Type mElementType;

            public MultiValueAsyncOperationCallback(Type returnType)
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
