using System;
using WampSharp.V2;
using WampSharp.V2.PubSub;

namespace WampSharp.Samples.Subscriber
{
    public interface IOptionsSubscriber
    {
        [WampTopic("com.myapp.topic1")]
        void OnEvent(int value);
    }

    public class OptionsSubscriber : IOptionsSubscriber
    {
        public void OnEvent(int value)
        {
            WampEventContext context = WampEventContext.Current;

            Console.WriteLine($"Got event, publication ID {context.PublicationId}, publisher {context.EventDetails.Publisher}: {value}");
        }
    }
}