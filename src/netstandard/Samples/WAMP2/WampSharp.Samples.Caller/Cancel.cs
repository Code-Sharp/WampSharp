using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Caller
{
    public interface ICancellableServiceProxy
    {
        [WampProcedure("com.myapp.cancellableop")]
        Task<int> LongCancellableOpAsync(int n, CancellationToken token);
    }

    [Command("cancel")]
    public class CancelCommand : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            ICancellableServiceProxy proxy = 
                channel.RealmProxy.Services.GetCalleeProxy<ICancellableServiceProxy>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task<int> invocationTask = proxy.LongCancellableOpAsync(4096, cancellationTokenSource.Token);

            await Task.Delay(1000).ConfigureAwait(false);

            // Cancel the operation
            cancellationTokenSource.Cancel();

            try
            {
                await invocationTask.ConfigureAwait(false);
            }
            catch (WampException ex)
            {
                Console.WriteLine($"Call was canceled. Details: Error: {ex.ErrorUri}, [{string.Join(", ", ex.Arguments ?? Enumerable.Empty<object>())}], {{{string.Join(", ", ex.ArgumentsKeywords?.Select(x => $"{x.Key} : {x.Value}") ?? Enumerable.Empty<object>())}}}");
            }
        }
    }
}