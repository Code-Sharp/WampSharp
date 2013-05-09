using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Client
{
    public class WampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient> : IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient>
    {
        private readonly IWampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient> mIncomingHandlerBuilder;

        public WampServerProxyOutgoingMessageHandlerBuilder(IWampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient> incomingHandlerBuilder)
        {
            mIncomingHandlerBuilder = incomingHandlerBuilder;
        }

        public IWampOutgoingMessageHandler<TMessage> Build(TRawClient client, IWampConnection<TMessage> connection)
        {
            IWampIncomingMessageHandler<TMessage> incomingMessageHandler =  mIncomingHandlerBuilder.Build(client, connection);
            return new WampServerProxyHandler<TMessage>(connection, incomingMessageHandler);
        }
    }
}