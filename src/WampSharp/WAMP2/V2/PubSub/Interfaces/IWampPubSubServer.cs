using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public interface IWampPubSubServer<TMessage> : IWampBroker<TMessage>
    {
    }
}