using System;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Core.Listener.ClientBuilder
{
    /// <summary>
    /// An implementation of <see cref="IWampClientBuilder{TMessage,TClient}"/>
    /// that is specific to WAMPv1.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampClientBuilder<TMessage> : IWampClientBuilder<TMessage, IWampClient>
    {
        #region Members

        private readonly IWampClientContainer<TMessage, IWampClient> mContainer;
        private readonly IWampOutgoingRequestSerializer mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage> mOutgoingHandlerBuilder;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WampClientBuilder{TMessage}"/>.
        /// </summary>
        /// <param name="outgoingSerializer">A <see cref="IWampOutgoingRequestSerializer"/>
        /// used to serialize message calls into <see cref="WampMessage{TMessage}"/>s</param>
        /// <param name="outgoingHandlerBuilder">An <see cref="IWampOutgoingMessageHandlerBuilder{TMessage}"/> used to build
        /// a <see cref="IWampOutgoingMessageHandler"/> per connection.</param>
        /// <param name="container">A <see cref="IWampClientContainer{TMessage,TClient}"/> that contains all clients.</param>
        public WampClientBuilder(IWampOutgoingRequestSerializer outgoingSerializer, IWampOutgoingMessageHandlerBuilder<TMessage> outgoingHandlerBuilder, IWampClientContainer<TMessage, IWampClient> container)
        {
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
            mContainer = container;
        }

        #endregion

        public IWampClient Create(IWampConnection<TMessage> connection)
        {
            IWampOutgoingMessageHandler outgoingHandler =
                mOutgoingHandlerBuilder.Build(connection);

            WampConnectionMonitor<TMessage> monitor =
                new WampConnectionMonitor<TMessage>(connection);

            IDisposable disposable =
                new WampClientContainerDisposable<TMessage, IWampClient>
                    (mContainer, connection);

            WampClientProxy result =
                new WampClientProxy(outgoingHandler,
                                    mOutgoingSerializer,
                                    monitor,
                                    disposable);

            result.SessionId = (string) mContainer.GenerateClientId(result);

            monitor.Client = result;

            return result;
        }
    }
}