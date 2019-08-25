using System;
using WampSharp.V2;
using WampSharp.V2.PubSub;

namespace WampSharp.Samples.Subscriber
{
    public class OptionsSubscriber : IBasicSubscriber
    {
        public void OnEvent(int value)
        {
            WampEventContext context = WampEventContext.Current;

            Console.WriteLine($"Got event, publication ID {context.PublicationId}, publisher {context.EventDetails.Publisher}: {value}");
        }
    }
}