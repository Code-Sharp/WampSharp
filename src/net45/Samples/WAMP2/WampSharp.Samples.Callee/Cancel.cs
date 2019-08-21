using System;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Callee
{
    public class CancellableOpService
    {
        [WampProcedure("com.myapp.cancellableop")]
        public async Task<int> CancellableOp(int n, CancellationToken token)
        {
            for (int i = 0; i < n; i++)
            {
                if (token.IsCancellationRequested)
                {
                    throw new WampRpcCanceledException($" {i * 100.0 / n}% of the work was done");
                }

                await Task.Delay(100, token);
            }

            return n;
        }

        [WampProgressiveResultProcedure]
        [WampProcedure("com.myapp.cancellableopprogress")]
        public async Task<int> LongCancellableOp(int n, IProgress<int> progress, CancellationToken token)
        {
            for (int i = 0; i < n; i++)
            {
                if (token.IsCancellationRequested)
                {
                    throw new WampRpcCanceledException($" {i * 100.0 / n}% of the work was done");
                }

                progress.Report(i);
                await Task.Delay(100, token);
            }

            return n;
        }
    }
}