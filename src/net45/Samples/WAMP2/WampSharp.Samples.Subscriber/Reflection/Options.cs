using System;
using System.Threading.Tasks;
using WampSharp.V2;

namespace WampSharp.Samples.Subscriber.Reflection
{
    class OptionsProgram
    {
        public static async Task RunAsync(IWampChannel channel)
        {
            await Program.Run<OptionsSubscriber>(channel);
        }
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