using WampSharp.Core.Contracts.V1;

namespace WampSharp.PubSub
{
    public interface IWampServerProxyFactory<TMessage>
    {
        IWampServer Create(IWampPubSubClient<TMessage> client);
    }
}