using System;
using WampSharp.V2;

namespace WampSharp.Samples.Subscriber.Reflection
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