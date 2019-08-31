using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Caller
{
    public interface ITimeServiceProxy
    {
        [WampProcedure("com.timeservice.now")]
        Task<string> NowAsync();
    }

    [Command("timeservice")]
    public class TimeServiceCommand : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            ITimeServiceProxy proxy = 
                channel.RealmProxy.Services.GetCalleeProxy<ITimeServiceProxy>();

            void PrintException(WampException ex)
            {
                Console.WriteLine($"Error: {ex.ErrorUri}, [{string.Join(", ", ex.Arguments ?? Enumerable.Empty<object>())}], {{{string.Join(", ", ex.ArgumentsKeywords?.Select(x => $"{x.Key} : {x.Value}") ?? Enumerable.Empty<object>())}}}");
            }

            try
            {
                string result = await proxy.NowAsync().ConfigureAwait(false);
                Console.WriteLine($"Current time from time service: {result}");
            }
            catch (WampException ex)
            {
                PrintException(ex);
            }

            Console.ReadLine();
        }
    }
}