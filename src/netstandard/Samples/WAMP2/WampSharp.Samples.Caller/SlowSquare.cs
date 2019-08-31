using System;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Caller
{
    public interface ISlowSquareServiceProxy
    {
        [WampProcedure("com.math.slowsquare")]
        Task<int> SlowSquareAsync(int x);
        
        [WampProcedure("com.math.square")]
        Task<int> SquareAsync(int x);
    }

    [Command("slowsquare")]
    public class SlowSquareSample : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            ISlowSquareServiceProxy proxy = 
                channel.RealmProxy.Services.GetCalleeProxy<ISlowSquareServiceProxy>();

            DateTime quickStartTime = DateTime.Now;

            int quickResult = await proxy.SquareAsync(3).ConfigureAwait(false);

            Console.WriteLine($"Quick Square: {quickResult} in {(DateTime.Now - quickStartTime).TotalMilliseconds}");

            DateTime slowStartTime = DateTime.Now;

            int slowResult = await proxy.SlowSquareAsync(3).ConfigureAwait(false);

            Console.WriteLine($"Slow Square: {slowResult} in {(DateTime.Now - slowStartTime).TotalMilliseconds}");

            Console.ReadLine();
        }
    }
}