using System;
using System.Threading.Tasks;
using WampSharp.Samples.Subscriber.Rx;
using WampSharp.V2;

namespace WampSharp.Samples.Subscriber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IWampChannel channel = SamplesArgumentParser.CreateWampChannel(args);

            await OptionsProgram.RunAsync(channel);
        }        
    }
}