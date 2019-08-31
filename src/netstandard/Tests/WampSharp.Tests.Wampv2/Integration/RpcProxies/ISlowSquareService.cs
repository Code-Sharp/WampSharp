using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcProxies
{
    public interface ISlowSquareService
    {
        [WampProcedure("com.math.slowsquare")]
        int SlowSquare(int x);

        [WampProcedure("com.math.square")]
        int Square(int x);

        [WampProcedure("com.math.slowsquare")]
        Task<int> SlowSquareAsync(int x);

        [WampProcedure("com.math.square")]
        Task<int> SquareAsync(int x);
    }
}