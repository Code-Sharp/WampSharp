using WampSharp.Core.Contracts.V1;

namespace WampSharp.Rpc
{
    public interface IWampServerProxyFactory<TMessage>
    {
        IWampServer Create(IWampRpcClient<TMessage> client);
    }
}