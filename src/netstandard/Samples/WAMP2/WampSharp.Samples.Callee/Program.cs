using System;
using System.Threading.Tasks;
using CliFx;
using WampSharp.V2;
using WampSharp.Samples.Common;

namespace WampSharp.Samples.Callee
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new CliApplicationBuilder()
                  .AddCommandsFromThisAssembly()
                  .Build()
                  .RunAsync(args);
        }
    }

    public class CalleeCommand<TService> : SampleCommand
        where TService : new()
    {
        protected async override Task RunAsync(IWampChannel channel)
        {
            await channel.Open();

            TService service = new TService();

            await using (IAsyncDisposable disposable =
                await channel.RealmProxy.Services.RegisterCallee(service))
            {
                Console.WriteLine($"Registered {typeof(TService).Name}!");

                await Task.Yield();

                Console.ReadLine();
            }
        }
    }
}