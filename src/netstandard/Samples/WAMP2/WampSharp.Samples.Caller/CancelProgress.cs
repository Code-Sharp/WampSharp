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
    public interface ICancellableProgressServiceProxy
    {
        [WampProcedure("com.myapp.cancellableopprogress")]
        [WampProgressiveResultProcedure]
        Task<int> LongCancellableOpAsync(int n, IProgress<int> progress, CancellationToken token);
    }

    [Command("cancelprogress")]
    public class CancelProgressCommand : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            ICancellableProgressServiceProxy proxy = 
                channel.RealmProxy.Services.GetCalleeProxy<ICancellableProgressServiceProxy>();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            IProgress<int> progress = new Progress<int>(i => Console.WriteLine("Got progress " + i));

            Task<int> invocationTask = proxy.LongCancellableOpAsync(4096, progress, cancellationTokenSource.Token);

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