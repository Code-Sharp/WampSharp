using System;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;

namespace WampSharp.Samples.Subscriber.Rx
{
    [Command("options rx")]
    public class OptionsCommand : SampleCommand 
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            await Task.Yield();

            IWampSubject topic = channel.RealmProxy.Services.GetSubject("com.myapp.topic1");

            using (IDisposable disposable =
                topic.Subscribe(value => Console.WriteLine($"Got event, publication ID {value.PublicationId}, publisher {value.Details.Publisher}: {value.Arguments[0].Deserialize<int>()}"),
                                ex => Console.WriteLine($"An error has occurred {ex}")))
            {
                Console.WriteLine("Hit enter to unsubscribe");

                Console.ReadLine();
            }

            Console.WriteLine("Unsubscribed");

            Console.ReadLine();
        }
    }
}