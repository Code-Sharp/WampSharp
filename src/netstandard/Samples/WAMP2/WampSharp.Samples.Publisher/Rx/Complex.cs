using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CliFx.Attributes;
using Newtonsoft.Json;
using WampSharp.Samples.Common;
using WampSharp.V2;

namespace WampSharp.Samples.Publisher.Rx
{
    [Command("complex rx")]
    public class ComplexCommand : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            await Task.Yield();

            IWampSubject heartbeatSubject =
                channel.RealmProxy.Services.GetSubject("com.myapp.heartbeat");

            var topic2Subject =
                channel.RealmProxy.Services.GetSubject("com.myapp.topic2", new Topic2TupleConverter());

            IObservable<int> timer =
                Observable.Timer(TimeSpan.FromSeconds(0),
                                 TimeSpan.FromSeconds(1)).Select((x, i) => i);

            Random random = new Random();

            WampEvent emptyEvent = new WampEvent();

            using (IDisposable heartbeatDisposable = timer.Select(value => emptyEvent).Subscribe(heartbeatSubject))
            {
                (int number1, int number2, string c, MyClass d) Topic2Selector(int value)
                {
                    return (number1: random.Next(0, 100),
                            number2: 23,
                            c: "Hello",
                            d: new MyClass()
                               {
                                   Counter = value,
                                   Foo = new int[] {1, 2, 3}
                               });
                }

                using (IDisposable topic2Disposable =
                    timer.Select(value => Topic2Selector(value)).Subscribe(topic2Subject))
                {
                    Console.WriteLine("Hit enter to stop publishing");

                    Console.ReadLine();
                }
            }

            Console.WriteLine("Stopped publishing");

            Console.ReadLine();
        }
    }

    internal class Topic2TupleConverter : 
        WampEventValueTupleConverter<(int number1, int number2, string c, MyClass d)>
    {
    }

    internal class MyClass
    {
        [JsonProperty("counter")]
        public int Counter { get; set; }

        [JsonProperty("foo")]
        public int[] Foo { get; set; }
    }
}