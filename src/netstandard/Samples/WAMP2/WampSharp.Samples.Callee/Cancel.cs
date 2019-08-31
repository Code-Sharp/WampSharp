using System;
using System.Threading;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Callee
{
    [Command("cancel")]
    public class CancelCommand : CalleeCommand<CancellableOpService>
    {
    }

    public class CancellableOpService
    {
        [WampProcedure("com.myapp.cancellableop")]
        public async Task<int> CancellableOp(int n, CancellationToken token)
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Processing {i * 100.0/n}");

                if (token.IsCancellationRequested)
                {
                    throw new WampRpcCanceledException($" {i * 100.0 / n}% of the work was done");
                }

                await Task.Delay(100, token);
            }

            return n;
        }
    }
}