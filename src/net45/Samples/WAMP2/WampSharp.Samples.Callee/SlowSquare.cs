using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Callee
{
    [Command("slowsquare")]
    public class SlowSquareCommand : CalleeCommand<SlowSquareService>
    {
    }

    public class SlowSquareService
    {
        [WampProcedure("com.math.slowsquare")]
        public async Task<int> SlowSquare(int x)
        {
            await Task.Delay(1000);
            return x * x;
        }

        [WampProcedure("com.math.square")]
        public int Square(int x)
        {
            return x * x;
        }        
    }
}