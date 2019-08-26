using System.Threading.Tasks;
using WampSharp.V2;

namespace WampSharp.Samples.Caller
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IWampChannel channel = SamplesArgumentParser.CreateWampChannel(args);

            await channel.Open();

            await ArgumentsProgram.RunAsync(channel).ConfigureAwait(false);
        }
    }
}