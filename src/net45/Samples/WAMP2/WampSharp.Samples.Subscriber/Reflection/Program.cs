using System;
using System.Threading.Tasks;
using WampSharp.V2;

namespace WampSharp.Samples.Subscriber.Reflection
{
    class Program
    {
        public static async Task Run<TService>(IWampChannel channel) where TService : new()
        {
            await channel.Open();

            TService service = new TService();

            await using (IAsyncDisposable disposable =
                await channel.RealmProxy.Services.RegisterSubscriber(service))
            {
                Console.WriteLine($"Subscribered {typeof(TService).Name}!");

                await Task.Yield();

                Console.WriteLine("Hit enter to unsubscribe");

                Console.ReadLine();
            }

            Console.WriteLine("Unsubscribed");

            Console.ReadLine();
        }
    }
}