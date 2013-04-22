using WampSharp.Core.Proxy;

namespace WampSharp.Core.Listener
{
    public interface IWampOutgoingMessageHandlerBuilder<TMessage, TConnection>
    {
        IWampOutgoingMessageHandler<TMessage> Build(TConnection connection);
    }
}