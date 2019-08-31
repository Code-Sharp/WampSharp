using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CliFx.Attributes;
using Newtonsoft.Json;
using WampSharp.Samples.Common;
using WampSharp.V2;
using WampSharp.V2.PubSub;

namespace WampSharp.Samples.Publisher.Reflection
{
    [Command("complex reflection")]
    public class ComplexCommand : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
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

    public class MyClass
    {
        [JsonProperty("counter")]
        public int Counter { get; set; }

        [JsonProperty("foo")]
        public int[] Foo { get; set; }
    }

    public delegate void MyPublicationDelegate(int number1, int number2, string c, MyClass d);

    public interface IComplexPublisher
    {
        [WampTopic("com.myapp.heartbeat")]
        event Action Heartbeat;

        [WampTopic("com.myapp.topic2")]
        event MyPublicationDelegate MyEvent;
    }

    public class ComplexPublisher : IComplexPublisher, IDisposable
    {
        private readonly Random mRandom = new Random();
        private IDisposable mSubscription;

        public ComplexPublisher()
        {
            mSubscription =
                Observable.Timer(TimeSpan.FromSeconds(0),
                                 TimeSpan.FromSeconds(1)).Select((x, i) => i)
                          .Subscribe(x => OnTimer(x));
        }

        private void OnTimer(int value)
        {
            RaiseHeartbeat();

            RaiseMyEvent(mRandom.Next(0, 100),
                         23,
                         "Hello",
                         new MyClass()
                         {
                             Counter = value,
                             Foo = new int[] {1, 2, 3}
                         });
        }

        private void RaiseHeartbeat()
        {
            Heartbeat?.Invoke();
        }

        private void RaiseMyEvent(int number1, int number2, string c, MyClass d)
        {
            MyEvent?.Invoke(number1, number2, c, d);
        }

        public event Action Heartbeat;

        public event MyPublicationDelegate MyEvent;

        public void Dispose()
        {
            mSubscription?.Dispose();
            mSubscription = null;
        }
    }
}