using System;
using System.Threading.Tasks;
using WampSharp.V2;

namespace WampSharp.Samples.Publisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IWampChannel channel = SamplesArgumentParser.CreateWampChannel(args);

            await channel.Open().ConfigureAwait(false);

            ComplexPublisher publisher = new ComplexPublisher();

            using (IDisposable disposable =
                channel.RealmProxy.Services.RegisterPublisher(publisher))
            {
                Console.WriteLine("Hit enter to stop publishing");

                Console.ReadLine();
            }

            Console.WriteLine("Stopped publishing");

            Console.ReadLine();
        }
    }
}
