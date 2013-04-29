using WampSharp.Core.Contracts;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Client
{
    public interface IWampServerProxyOutgoingMessageHandlerBuilder<TMessage>
    {
        IWampOutgoingMessageHandler<TMessage> Build(IWampClient<TMessage> client, IWampConnection<TMessage> connection);
    }
}