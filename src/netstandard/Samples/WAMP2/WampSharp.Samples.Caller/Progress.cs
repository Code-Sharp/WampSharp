using System;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Caller
{
    public interface ILongOpServiceProxy
    {
        [WampProcedure("com.myapp.longop")]
        [WampProgressiveResultProcedure]
        Task<int> LongOpAsync(int n, IProgress<int> progress);
    }

    [Command("progress")]
    public class ProgressCommand : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            ILongOpServiceProxy proxy = 
                channel.RealmProxy.Services.GetCalleeProxy<ILongOpServiceProxy>();

            Progress<int> progress =
                new Progress<int>(i => Console.WriteLine("Got progress " + i));

            int result = await proxy.LongOpAsync(10, progress).ConfigureAwait(false);

            Console.WriteLine("Got result " + result);
        }
    }
}