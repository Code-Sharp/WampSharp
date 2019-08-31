using System;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Samples.Publisher.Reflection
{
    [Command("options reflection")]
    public class OptionsCommand : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            BasicPublisher publisher = new BasicPublisher();

            using (IDisposable disposable =
                channel.RealmProxy.Services.RegisterPublisher
                    (publisher,
                     new PublisherRegistrationInterceptor
                         (new PublishOptions {DiscloseMe = true})))
            {
                Console.WriteLine("Hit enter to stop publishing");

                Console.ReadLine();
            }

            Console.WriteLine("Stopped publishing");

            Console.ReadLine();
        }        
    }
}