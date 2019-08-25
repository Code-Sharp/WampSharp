using System.Threading.Tasks;
using WampSharp.V2;

namespace WampSharp.Samples.Publisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IWampChannel channel = SamplesArgumentParser.CreateWampChannel(args);

            await ComplexProgram.RunAsync(channel);
        }
    }
}
