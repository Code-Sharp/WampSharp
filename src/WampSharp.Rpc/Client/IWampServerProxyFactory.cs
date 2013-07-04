using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Listener;

namespace WampSharp.Rpc
{
    public interface IWampServerProxyFactory<TMessage>
    {
        IWampServer Create(IWampRpcClient<TMessage> client, IWampConnection<TMessage> connection);
    }
}