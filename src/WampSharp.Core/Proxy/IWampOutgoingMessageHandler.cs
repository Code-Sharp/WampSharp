using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.Core.Proxy
{
    public interface IWampOutgoingMessageHandler<TMessage>
    {
        void Handle(IWampClient client, WampMessage<TMessage> message);
    }
}