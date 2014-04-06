using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal interface ISubscribeRequest<TMessage>
    {
        IWampClient<TMessage> Client { get; }

        void Subscribed(long subscriptionId);
    }
}