using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Listener;

namespace WampSharp.PubSub.Client
{
    public interface IWampServerProxyFactory<TMessage>
    {
        IWampServer Create(IWampPubSubClient<TMessage> client, IWampConnection<TMessage> connection);
    }
}