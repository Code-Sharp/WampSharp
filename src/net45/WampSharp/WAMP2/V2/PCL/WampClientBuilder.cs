#if PCL
using System;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core.Listener.ClientBuilder
{
    /// <summary>
    /// An implementation of <see cref="IWampClientBuilder{TMessage,TClient}"/>
    /// that is specific to WAMPv2.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampClientBuilder<TMessage> : IWampClientBuilder<TMessage, IWampClientProxy<TMessage>>
    {
        #region Members

        private readonly IWampClientContainer<TMessage, IWampClientProxy<TMessage>> mContainer;
        private readonly IWampOutgoingRequestSerializer mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage> mOutgoingHandlerBuilder;
        private readonly IWampIdGenerator mSessionIdGenerator;
        private readonly IWampBinding<TMessage> mBinding;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WampClientBuilder{TMessage}"/>.
        /// </summary>
        /// <param name="sessionIdGenerator">A given <see cref="IWampIdGenerator"/> used in order
        /// to generate session ids for clients.</param>
        /// <param name="outgoingSerializer">A <see cref="IWampOutgoingRequestSerializer"/>
        /// used to serialize message calls into <see cref="WampMessage{TMessage}"/>s</param>
        /// <param name="outgoingHandlerBuilder">An <see cref="IWampOutgoingMessageHandlerBuilder{TMessage}"/> used to build
        /// a <see cref="IWampOutgoingMessageHandler"/> per connection.</param>
        /// <param name="container">A <see cref="IWampClientContainer{TMessage,TClient}"/> that contains all clients.</param>
        public WampClientBuilder(IWampIdGenerator sessionIdGenerator, IWampOutgoingRequestSerializer outgoingSerializer, IWampOutgoingMessageHandlerBuilder<TMessage> outgoingHandlerBuilder, IWampClientContainer<TMessage, IWampClientProxy<TMessage>> container, IWampBinding<TMessage> binding)
        {
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
            mContainer = container;
            mBinding = binding;
            mSessionIdGenerator = sessionIdGenerator;
        }

        #endregion

        public IWampClientProxy<TMessage> Create(IWampConnection<TMessage> connection)
        {
            IWampOutgoingMessageHandler outgoingHandler =
                mOutgoingHandlerBuilder.Build(connection);

            WampConnectionMonitor<TMessage> monitor =
                new WampConnectionMonitor<TMessage>(connection);

            IDisposable disposable =
                new WampClientContainerDisposable<TMessage, IWampClientProxy<TMessage>>
                (mContainer, connection);

            WampClientProxy<TMessage> result =
                new WampClientProxy<TMessage>(outgoingHandler,
                                              mOutgoingSerializer,
                                              monitor,
                                              disposable);

            result.Session = mSessionIdGenerator.Generate();
            result.Binding = mBinding;
            monitor.Client = result;

            return result;
        }
    }
}

#endif