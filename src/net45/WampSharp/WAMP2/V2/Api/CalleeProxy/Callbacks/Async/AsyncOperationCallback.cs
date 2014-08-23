using System;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract class AsyncOperationCallback : IWampRawRpcOperationCallback
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
