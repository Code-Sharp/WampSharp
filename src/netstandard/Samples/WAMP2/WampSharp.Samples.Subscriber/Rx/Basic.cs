using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;

namespace WampSharp.Samples.Subscriber.Rx
{
    [Command("basic rx")]
    public class BasicCommand : SampleCommand 
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            await Task.Yield();

            ISubject<int> topic = channel.RealmProxy.Services.GetSubject<int>("com.myapp.topic1");

            using (IDisposable disposable =
                topic.Subscribe(value => Console.WriteLine($"Got event: {value}"),
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