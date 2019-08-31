using System;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal class WampPendingRequestBase<TMessage, TResult> : IWampPendingRequest
    {
        private readonly TaskCompletionSource<TResult> mTaskCompletionSource = new TaskCompletionSource<TResult>();
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampPendingRequestBase(IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
        }

        public long RequestId
        {
            get;
            set;
        }

        protected Task<TResult> Task => mTaskCompletionSource.Task;

        protected void Complete(TResult result)
        {
            mTaskCompletionSource.SetResult(result);
        }

        // TODO: Don't repeat yourself
        public void Error(TMessage details, string error)
        {
            WampException exception = ErrorExtractor.Error(mFormatter, details, error);
            SetException(exception);
        }

        public void Error(TMessage details, string error, TMessage[] arguments)
        {
            var exception = ErrorExtractor.Error(mFormatter, details, error, arguments);
            SetException(exception);
        }

        public void Error(TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            WampException exception = ErrorExtractor.Error(mFormatter, details, error, arguments, argumentsKeywords);

            SetException(exception);
        }

        public void SetException(Exception exception)
        {
            mTaskCompletionSource.TrySetException(exception);
        }
    }
}