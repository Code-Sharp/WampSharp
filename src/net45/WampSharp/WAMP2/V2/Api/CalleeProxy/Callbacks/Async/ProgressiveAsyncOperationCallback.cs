#if !NET40

using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.CalleeProxy
{
    class ProgressiveAsyncOperationCallback<TResult> : AsyncOperationCallback
    {
        private readonly IProgress<TResult> mProgress;

        public ProgressiveAsyncOperationCallback(IProgress<TResult> progress,
            IOperationResultExtractor extractor) :
                base(extractor)
        {
            mProgress = progress;
        }

        protected override void SetResult(ResultDetails details, object result)
        {
            if (details.Progress == true)
            {
                mProgress.Report((TResult) result);
            }
            else
            {
                base.SetResult(details, result);                
            }
        }
    }
}

#endif