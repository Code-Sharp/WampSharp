using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    public interface IWampRpcOperationRegistrationProxy
    {
        Task Register(IWampRpcOperation operation, object options);

        Task Unregister(IWampRpcOperation operation);         
    }
}