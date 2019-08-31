using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcProxies
{
    public interface IArgumentsService
    {
        [WampProcedure("com.arguments.ping")]
        void Ping();

        [WampProcedure("com.arguments.add2")]
        int Add2(int a, int b);

        [WampProcedure("com.arguments.stars")]
        string Stars(string nick = "somebody", int stars = 0);

        [WampProcedure("com.arguments.orders")]
        string[] Orders(string product, int limit = 5);

        [WampProcedure("com.arguments.ping")]
        Task PingAsync();

        [WampProcedure("com.arguments.add2")]
        Task<int> Add2Async(int a, int b);

        [WampProcedure("com.arguments.stars")]
        Task<string> StarsAsync(string nick = "somebody", int stars = 0);

        [WampProcedure("com.arguments.orders")]
        Task<string[]> OrdersAsync(string product, int limit = 5);
    }
}