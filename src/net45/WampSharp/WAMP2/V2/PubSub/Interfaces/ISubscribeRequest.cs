using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal interface ISubscribeRequest<TMessage>
    {
        IWampClientProxy<TMessage> Client { get; }

        void Subscribed(long subscriptionId);
    }
}