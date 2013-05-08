using WampSharp.Core.Contracts;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Client
{
    public class WampServerProxyOutgoingMessageHandlerBuilder<TMessage> : IWampServerProxyOutgoingMessageHandlerBuilder<TMessage>
    {
        private readonly IWampServerProxyIncomingMessageHandlerBuilder<TMessage> mIncomingHandlerBuilder;

        public WampServerProxyOutgoingMessageHandlerBuilder(IWampServerProxyIncomingMessageHandlerBuilder<TMessage> incomingHandlerBuilder)
        {
            mIncomingHandlerBuilder = incomingHandlerBuilder;
        }

        public IWampOutgoingMessageHandler<TMessage> Build(IWampClient<TMessage> client, IWampConnection<TMessage> connection)
        {
            IWampIncomingMessageHandler<TMessage> incomingMessageHandler =  mIncomingHandlerBuilder.Build(client, connection);
            return new WampServerProxyHandler<TMessage>(connection, incomingMessageHandler);
        }
    }
}