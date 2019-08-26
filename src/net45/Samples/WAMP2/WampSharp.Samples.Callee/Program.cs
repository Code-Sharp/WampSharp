using System;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.Samples;

namespace WampSharp.Samples.Callee
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IWampChannel channel = SamplesArgumentParser.CreateWampChannel(args);

            await channel.Open();

            ArgumentsService service = new ArgumentsService();

            await using (IAsyncDisposable disposable =
                await channel.RealmProxy.Services.RegisterCallee(service))
            {
                Console.WriteLine($"Registered {service.GetType().Name}!");

                await Task.Yield();

                Console.ReadLine();
            }
        }
    }
}
