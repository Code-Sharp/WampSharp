using WampSharp.Core.Proxy;

namespace WampSharp.Core.Listener
{
    public interface IWampOutgoingMessageHandlerBuilder<TMessage>
    {
        IWampOutgoingMessageHandler<TMessage> Build(IWampConnection<TMessage> connection);
    }
}