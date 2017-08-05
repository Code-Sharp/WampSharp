using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.CalleeProxy
{
    class ProgressiveAsyncOperationCallback<TResult> : AsyncOperationCallback<TResult>
    {
        private readonly IProgress<TResult> mProgress;

        public ProgressiveAsyncOperationCallback(IProgress<TResult> progress,
            IOperationResultExtractor<TResult> extractor) :
                base(extractor)
        {
            mProgress = progress;
        }

        protected override void SetResult(ResultDetails details, TResult result)
        {
            if (details.Progress == true)
            {
                mProgress.Report(result);
            }
            else
            {
                base.SetResult(details, result);                
            }
        }
    }
}