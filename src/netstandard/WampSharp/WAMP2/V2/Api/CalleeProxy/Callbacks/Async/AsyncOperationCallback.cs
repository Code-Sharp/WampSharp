using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class AsyncOperationCallback<TResult> : IWampRawRpcOperationClientCallback
    {
        private readonly TaskCompletionSource<TResult> mTask = new TaskCompletionSource<TResult>();
        private readonly IOperationResultExtractor<TResult> mResultExtractor;

        public AsyncOperationCallback(IOperationResultExtractor<TResult> resultExtractor)
        {
            mResultExtractor = resultExtractor;
        }

        public Task<TResult> Task => mTask.Task;

        protected virtual void SetResult(ResultDetails details, TResult result)
        {
            mTask.SetResult(result);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
        {
            // TODO: throw exception if not nullable.
            SetResult(details, default(TResult));
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments)
        {
            SetResult(details, formatter, arguments);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            SetResult(details, formatter, arguments, argumentsKeywords);
        }

        protected virtual void SetResult<TMessage>(ResultDetails details, IWampFormatter<TMessage> formatter, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords = null)
        {
            void ReportAction(ResultDetails resultDetails, TResult result)
            {
                SetResult(resultDetails, result);
            }

            SetResultValue(details, formatter, arguments, argumentsKeywords, mResultExtractor,
                           ReportAction);
        }

        protected void SetResultValue<TResultType, TMessage>(ResultDetails details, IWampFormatter<TMessage> formatter, TMessage[] arguments, 
                                                      IDictionary<string, TMessage> argumentsKeywords,
                                                      IOperationResultExtractor<TResultType> extractor,
                                                      Action<ResultDetails, TResultType> setResultAction)
        {
            try
            {
                TResultType result = extractor.GetResult(formatter, arguments, argumentsKeywords);
                setResultAction(details, result);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
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
            mTask.TrySetException(exception);
        }
    }
}