using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcProxies
{
    public interface ITimeService
    {
        [WampProcedure("com.timeservice.now")]
        string UtcNow();

        [WampProcedure("com.timeservice.now")]
        Task<string> UtcNowAsync();
    }
}