using System;
using CliFx.Attributes;
using WampSharp.V2.PubSub;

namespace WampSharp.Samples.Subscriber.Reflection
{
    [Command("basic reflection")]
    public class BasicCommand : SubscriberCommand<BasicSubscriber>
    {
    }

    public interface IBasicSubscriber
    {
        [WampTopic("com.myapp.topic1")]
        void OnEvent(int value);
    }

    public class BasicSubscriber : IBasicSubscriber
    {
        public void OnEvent(int value)
        {
            Console.WriteLine($"Got event: {value}");
        }
    }
}