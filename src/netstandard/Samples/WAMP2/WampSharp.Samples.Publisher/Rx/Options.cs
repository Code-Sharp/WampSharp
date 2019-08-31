using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Samples.Publisher.Rx
{
    [Command("options rx")]
    public class OptionsProgram : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            await Task.Yield();

            IWampSubject topic = channel.RealmProxy.Services.GetSubject("com.myapp.topic1");

            IObservable<int> timer =
                Observable.Timer(TimeSpan.FromSeconds(0),
                                 TimeSpan.FromSeconds(1)).Select((x, i) => i);

            WampEvent TopicSelector(int value)
            {
                return new WampEvent()
                       {
                           Arguments = new object[] {value}, Options =
                               new PublishOptions() {DiscloseMe = true}
                       };
            }

            using (IDisposable disposable = timer.Select(value => TopicSelector(value)).Subscribe(topic))
            {
                Console.WriteLine("Hit enter to stop publishing");

                Console.ReadLine();
            }

            Console.WriteLine("Stopped publishing");

            Console.ReadLine();
        }
    }
}