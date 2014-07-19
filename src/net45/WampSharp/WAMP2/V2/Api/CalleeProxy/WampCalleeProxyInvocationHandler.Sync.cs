using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract partial class WampCalleeProxyInvocationHandler
    {
        private abstract class SyncCallbackBack : IWampRawRpcOperationCallback
        {
            private readonly ManualResetEvent mResetEvent = new ManualResetEvent(false);
            private WampException mException;

            public WampException Exception
            {
                get { return mException; }
            }

            public void Wait(TimeSpan timeSpan)
            {
                mResetEvent.WaitOne(timeSpan);
            }

            protected void ResultArrived()
            {
                mResetEvent.Set();
            }

            public abstract void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details);

            public abstract void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details,
                                                  TMessage[] arguments);

            public abstract void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details,
                                                  TMessage[] arguments, TMessage argumentsKeywords);

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
            {
                mException = ErrorExtractor.Error(formatter, details, error);
                ResultArrived();
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error,
                                        TMessage[] arguments)
            {
                mException = ErrorExtractor.Error(formatter, details, error, arguments);
                mResetEvent.Set();
            }

            public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error,
                                        TMessage[] arguments,
                                        TMessage argumentsKeywords)
            {
                mException = ErrorExtractor.Error(formatter, details, error, arguments, argumentsKeywords);
                mResetEvent.Set();
            }
        }

        private abstract class SyncCallback : SyncCallbackBack
        {
            protected readonly MethodInfoHelper mMethodInfoHelper;
            private readonly object[] mArguments;
            private object mResult;

            public SyncCallback(MethodInfoHelper methodInfoHelper, object[] arguments)
            {
                mMethodInfoHelper = methodInfoHelper;
                mArguments = arguments;
            }

            public object OperationResult
            {
                get { return mResult; }
            }

            public override void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details)
            {
                SetResult(null);
            }

            public override void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details,
                                                  TMessage[] arguments)
            {
                SetResult(formatter, arguments);
            }

            public override void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details,
                                                  TMessage[] arguments, TMessage argumentsKeywords)
            {
                IDictionary<string, TMessage> outOrRefParameters =
                    formatter.Deserialize<IDictionary<string, TMessage>>(argumentsKeywords);

                mMethodInfoHelper.PopulateOutOrRefValues
                    (formatter,
                     mArguments,
                     outOrRefParameters);

                SetResult(formatter, arguments);
            }

            protected abstract void SetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments);

            protected void SetResult(object result)
            {
                mResult = result;
                ResultArrived();
            }
        }

        private class SingleValueSyncCallback : SyncCallback
        {
            public SingleValueSyncCallback(MethodInfoHelper methodInfoHelper, object[] arguments)
                : base(methodInfoHelper, arguments)
            {
            }

            protected override void SetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
            {
                if (!mMethodInfoHelper.Method.HasReturnValue() || !arguments.Any())
                {
                    SetResult(null);
                }
                else
                {
                    Type deserializedType = mMethodInfoHelper.Method.ReturnType;
                    object deserialized = formatter.Deserialize(deserializedType, arguments[0]);
                    SetResult(deserialized);
                }
            }
        }

        private class MultiValueSyncCallback : SyncCallback
        {
            public MultiValueSyncCallback(MethodInfoHelper methodInfoHelper, object[] arguments)
                : base(methodInfoHelper, arguments)
            {
            }

            protected override void SetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
            {
                if (!arguments.Any())
                {
                    SetResult(null);
                }
                else
                {
                    Type arrayElementType = mMethodInfoHelper.Method.ReturnType.GetElementType();

                    Array result = Array.CreateInstance(arrayElementType, arguments.Length);

                    for (int i = 0; i < arguments.Length; i++)
                    {
                        TMessage current = arguments[i];
                        var currentDeserialized = formatter.Deserialize(arrayElementType, current);
                        result.SetValue(currentDeserialized, i);
                    }

                    SetResult(result);
                }
            }
        }
    }
}
