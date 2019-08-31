using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampServerProxyOutgoingMessageHandlerBuilder{TMessage,TRawClient}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TRawClient"></typeparam>
    public class WampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient> : IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient>
    {
        private readonly IWampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient> mIncomingHandlerBuilder;

        /// <summary>
        /// Creates an instance of <see cref="WampServerProxyOutgoingMessageHandlerBuilder{TMessage,TRawClient}"/>
        /// </summary>
        /// <param name="incomingHandlerBuilder">An <see cref="IWampServerProxyIncomingMessageHandlerBuilder{TMessage,TRawClient}"/>
        /// used in order to build incoming handler for callbacks.</param>
        public WampServerProxyOutgoingMessageHandlerBuilder(IWampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient> incomingHandlerBuilder)
        {
            mIncomingHandlerBuilder = incomingHandlerBuilder;
        }

        public IWampOutgoingMessageHandler Build(TRawClient client, IWampConnection<TMessage> connection)
        {
            IWampIncomingMessageHandler<TMessage> incomingMessageHandler =  mIncomingHandlerBuilder.Build(client, connection);
            return new WampServerProxyHandler<TMessage>(connection, incomingMessageHandler);
        }
    }
}