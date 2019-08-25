using System;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.PubSub;

namespace WampSharp.Samples.Subscriber.Reflection
{
    class BasicProgram
    {
        public static async Task RunAsync(IWampChannel channel)
        {
            await Program.Run<BasicSubscriber>(channel);
        }
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