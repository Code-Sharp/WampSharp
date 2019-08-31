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
    public interface IErrorServiceProxy
    {
        [WampProcedure("com.myapp.sqrt")]
        Task<double> Sqrt(double x);
        
        [WampProcedure("com.myapp.checkname")]
        Task CheckName(string name);
        
        [WampProcedure("com.myapp.compare")]
        Task Compare(int a, int b);
    }

    [Command("error")]
    public class ErrorCommand : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            IErrorServiceProxy proxy = 
                channel.RealmProxy.Services.GetCalleeProxy<IErrorServiceProxy>();

            void PrintException(WampException ex)
            {
                Console.WriteLine($"Error: {ex.ErrorUri}, [{string.Join(", ", ex.Arguments ?? Enumerable.Empty<object>())}], {{{string.Join(", ", ex.ArgumentsKeywords?.Select(x => $"{x.Key} : {x.Value}") ?? Enumerable.Empty<object>())}}}");
            }

            foreach (int value in new[] {2, 0, -2})
            {
                try
                {
                    double result = await proxy.Sqrt(value).ConfigureAwait(false);
                    Console.WriteLine($"Result: {result}");
                }
                catch (WampException ex)
                {
                    PrintException(ex);
                }
            }

            foreach (string name in new[] {"foo", "a", new string('*', 11), "Hello"})
            {
                try
                {
                    await proxy.CheckName(name).ConfigureAwait(false);
                    Console.WriteLine($"Received result");
                }
                catch (WampException ex)
                {
                    PrintException(ex);
                }
            }

            try
            {
                await proxy.Compare(3, 7).ConfigureAwait(false);
                Console.WriteLine($"Received result");
            }
            catch (WampException ex)
            {
                PrintException(ex);
            }

            Console.WriteLine("Exiting; we received only errors we expected.");
        }
    }
}