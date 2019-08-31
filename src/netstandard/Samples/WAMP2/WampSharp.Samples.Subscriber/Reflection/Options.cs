using System;
using CliFx.Attributes;
using WampSharp.V2;

namespace WampSharp.Samples.Subscriber.Reflection
{
    [Command("options reflection")]
    public class OptionsCommand : SubscriberCommand<OptionsSubscriber>
    {
    }

    public class OptionsSubscriber : IBasicSubscriber
    {
        public void OnEvent(int value)
        {
            WampEventContext context = WampEventContext.Current;

            Console.WriteLine($"Got event, publication ID {context.PublicationId}, publisher {context.EventDetails.Publisher}: {value}");
        }
    }
}