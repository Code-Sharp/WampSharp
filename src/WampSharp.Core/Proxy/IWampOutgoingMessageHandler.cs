using WampSharp.Core.Message;

namespace WampSharp.Core.Proxy
{
    public interface IWampOutgoingMessageHandler<TMessage>
    {
        void Handle(WampMessage<TMessage> message);
    }
}