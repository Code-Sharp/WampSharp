using System.Threading;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class CancellationTokenSourceInvocation : IWampCancellableInvocation
    {
        private readonly Task mTask;
        private readonly CancellationTokenSource mCancellationTokenSource;

        public CancellationTokenSourceInvocation(Task task,
                                                 CancellationTokenSource cancellationTokenSource)
        {
            mTask = task;
            mCancellationTokenSource = cancellationTokenSource;
        }

        public void Cancel(InterruptDetails details)
        {
            mCancellationTokenSource.Cancel();
        }

        public bool IsInvocationCompleted => mTask.IsCompleted;
    }
}