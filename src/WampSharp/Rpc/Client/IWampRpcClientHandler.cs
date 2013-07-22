using System.Threading.Tasks;

namespace WampSharp.Rpc.Client
{
    public interface IWampRpcClientHandler
    {
        object Handle(WampRpcCall<object> rpcCall);
        Task<object> HandleAsync(WampRpcCall<object> rpcCall);
    }
}