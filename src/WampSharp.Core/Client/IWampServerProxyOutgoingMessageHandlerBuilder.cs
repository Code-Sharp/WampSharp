using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Client
{
    public interface IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient>
    {
        IWampOutgoingMessageHandler<TMessage> Build(TRawClient client, IWampConnection<TMessage> connection);
    }
}