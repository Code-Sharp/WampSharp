using System;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Caller
{
    public interface IArgumentsServiceProxy
    {
        [WampProcedure("com.arguments.ping")]
        Task PingAsync();
    
        [WampProcedure("com.arguments.add2")]
        Task<int> Add2Async(int a, int b);

        [WampProcedure("com.arguments.stars")]
        Task<string> StarsAsync();

        [WampProcedure("com.arguments.stars")]
        Task<string> StarsAsync(int stars);

        [WampProcedure("com.arguments.stars")]
        Task<string> StarsAsync(string nick);

        [WampProcedure("com.arguments.stars")]
        Task<string> StarsAsync(string nick, int stars);

        [WampProcedure("com.arguments.orders")]
        Task<string[]> OrdersAsync(string product);

        [WampProcedure("com.arguments.orders")]
        Task<string[]> OrdersAsync(string product, int limit);
    }

    [Command("arguments")]
    public class ArgumentsSample : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            IArgumentsServiceProxy proxy = 
                channel.RealmProxy.Services.GetCalleeProxy<IArgumentsServiceProxy>();

            await proxy.PingAsync().ConfigureAwait(false);
            
            Console.WriteLine("Pinged!");

            int add2Result =
                await proxy.Add2Async(2, 3).ConfigureAwait(false);

            Console.WriteLine($"Add2:{add2Result}");

            string starResult = await proxy.StarsAsync().ConfigureAwait(false);

            Console.WriteLine($"Starred 1:{starResult}");

            starResult = await proxy.StarsAsync("Homer").ConfigureAwait(false);

            Console.WriteLine($"Starred 2:{starResult}");

            starResult = await proxy.StarsAsync(5).ConfigureAwait(false);

            Console.WriteLine($"Starred 3:{starResult}");

            starResult = await proxy.StarsAsync("Homer", 5).ConfigureAwait(false);

            Console.WriteLine($"Starred 4:{starResult}");

            string[] ordersResult = 
                await proxy.OrdersAsync("coffee").ConfigureAwait(false);

            Console.WriteLine($"Orders 1: [{string.Join(", ", ordersResult)}]");

            ordersResult = 
                await proxy.OrdersAsync("coffee", 10).ConfigureAwait(false);

            Console.WriteLine($"Orders 2: [{string.Join(", ", ordersResult)}]");

            Console.WriteLine("All finished.");

            await channel.Close(WampErrors.CloseNormal, new GoodbyeDetails());

            Console.ReadLine();
        }
    }
}