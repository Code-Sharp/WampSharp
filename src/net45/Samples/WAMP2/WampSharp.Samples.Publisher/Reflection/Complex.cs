using System;
using System.Reactive.Linq;
using Newtonsoft.Json;
using WampSharp.V2.PubSub;

namespace WampSharp.Samples
{
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

    public class ComplexPublisher : IComplexPublisher
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
    }
}