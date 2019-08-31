using System.Threading;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class CancellationTokenSourceInvocation : IWampCancellableInvocation
    {
        private readonly CancellationTokenSource mCancellationTokenSource;

        public CancellationTokenSourceInvocation(CancellationTokenSource cancellationTokenSource)
        {
            mCancellationTokenSource = cancellationTokenSource;
        }

        public void Cancel(InterruptDetails details)
        {
            mCancellationTokenSource.Cancel();
        }
    }
}