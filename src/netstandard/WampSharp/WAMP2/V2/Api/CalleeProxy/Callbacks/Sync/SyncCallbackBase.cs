using System;
using System.Collections.Generic;
using System.Threading;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract class SyncCallbackBase : IWampRawRpcOperationClientCallback
    {
        private readonly ManualResetEvent mWaitHandle = new ManualResetEvent(false);

        public Exception Exception { get; private set; }

        public WaitHandle WaitHandle => mWaitHandle;

        public void Wait(int timeout)
        {
            WaitHandle.WaitOne(timeout);
        }

        protected void ResultArrived()
        {
            mWaitHandle.Set();
        }

        public abstract void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details);

        public abstract void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments);

        public abstract void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
        {
            WampException exception = ErrorExtractor.Error(formatter, details, error);
            SetException(exception);
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error,
                                    TMessage[] arguments)
        {
            WampException exception = ErrorExtractor.Error(formatter, details, error, arguments);
            SetException(exception);
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error,
                                    TMessage[] arguments,
                                    TMessage argumentsKeywords)
        {
            WampException exception = ErrorExtractor.Error(formatter, details, error, arguments, argumentsKeywords);
            SetException(exception);
        }

        public void SetException(Exception exception)
        {
            Exception = exception;
            mWaitHandle.Set();
        }
    }
}
