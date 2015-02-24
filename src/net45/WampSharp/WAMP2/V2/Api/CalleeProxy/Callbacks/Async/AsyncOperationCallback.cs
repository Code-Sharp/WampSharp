using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class AsyncOperationCallback : IWampRawRpcOperationClientCallback
    {
        private readonly TaskCompletionSource<object> mTask = new TaskCompletionSource<object>();
        private readonly IOperationResultExtractor mExtractor;

        public AsyncOperationCallback(IOperationResultExtractor extractor)
        {
            mExtractor = extractor;
        }

        public Task<object> Task
        {
            get { return mTask.Task; }
        }

        protected virtual void SetResult(ResultDetails details, object result)
        {
            mTask.SetResult(result);
        }

        protected virtual object GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
        {
            return mExtractor.GetResult(formatter, arguments);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
        {
            SetResult(details, null);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments)
        {
            SetResult(details, formatter, arguments);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            SetResult(details, formatter, arguments);
        }

        private void SetResult<TMessage>(ResultDetails details, IWampFormatter<TMessage> formatter, TMessage[] arguments)
        {
            object result = GetResult(formatter, arguments);
            SetResult(details, result);
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
        {
            Exception exception = ErrorExtractor.Error(formatter, details, error);
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
            mTask.SetException(exception);
        }
    }
}