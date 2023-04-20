using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.CalleeProxy
{
    class ProgressiveAsyncOperationCallback<TProgress, TResult> : AsyncOperationCallback<TResult>
    {
        private readonly IProgress<TProgress> mProgress;
        private readonly IOperationResultExtractor<TProgress> mProgressTypeExtractor;

        public ProgressiveAsyncOperationCallback(IProgress<TProgress> progress,
                                                 IOperationResultExtractor<TProgress> progressExtractor,
                                                 IOperationResultExtractor<TResult> resultExtractor) :
                base(resultExtractor)
        {
            mProgressTypeExtractor = progressExtractor;
            mProgress = progress;
        }

        protected override void SetResult<TMessage>(ResultDetails details, IWampFormatter<TMessage> formatter, TMessage[] arguments,
                                                    IDictionary<string, TMessage> argumentsKeywords = null)
        {
            if (details.Progress == true)
            {
                void ReportAction(ResultDetails resultDetails, TProgress progressValue)
                {
                    mProgress.Report(progressValue);
                }

                base.SetResultValue(details,formatter,arguments,argumentsKeywords,
                                    mProgressTypeExtractor, ReportAction);
            }
            else
            {
                base.SetResult(details, formatter, arguments, argumentsKeywords);
            }
        }
    }
}