using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal interface IUnsubscribeRequest<TMessage>
    {
        IWampClient<TMessage> Client { get; }

        void Unsubscribed();
    }
}