using System;
using System.Threading;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Callee
{
    [Command("cancelprogress")]
    public class CancelProgress : CalleeCommand<CancellableProgressService>
    {
    }

    public class CancellableProgressService
    {
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