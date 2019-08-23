using System;
using Newtonsoft.Json;
using WampSharp.V2;
using WampSharp.V2.PubSub;

namespace WampSharp.Samples.Subscriber
{
    public interface IComplexSubscriber
    {
        [WampTopic("com.myapp.heartbeat")]
        void OnHeartbeat();

        [WampTopic("com.myapp.topic2")]
        void OnTopic2(int number1, int number2, string c, MyClass d);
    }

    public class ComplexSubscriber : IComplexSubscriber
    {
        public void OnHeartbeat()
        {
            long publicationId = WampEventContext.Current.PublicationId;
            Console.WriteLine("Got heartbeat (publication ID " + publicationId + ")");
        }

        public void OnTopic2(int number1, int number2, string c, MyClass d)
        {
            Console.WriteLine("Got event: number1:{0}, number2:{1}, c:{2}, d:{3}",
                              number1, number2, c, d);
        }
    }

    public class MyClass
    {
        [JsonProperty("counter")]
        public int Counter { get; set; }

        [JsonProperty("foo")]
        public int[] Foo { get; set; }

        public override string ToString()
        {
            return string.Format("counter: {0}, foo: [{1}]",
                                 Counter,
                                 string.Join(", ", Foo));
        }
    }
}