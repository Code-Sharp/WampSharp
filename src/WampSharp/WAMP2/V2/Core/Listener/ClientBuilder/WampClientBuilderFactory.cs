using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core.Listener.ClientBuilder
{
    /// <summary>
    /// An implementation of <see cref="IWampClientBuilderFactory{TMessage,TClient}"/>
    /// using <see cref="WampClientBuilder{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampClientBuilderFactory<TMessage> : IWampClientBuilderFactory<TMessage, IWampClient>
    {
        private readonly IWampIdGenerator mSessionIdGenerator;
        private readonly IWampOutgoingRequestSerializer<TMessage> mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage> mOutgoingHandlerBuilder;

        /// <summary>
        /// Creates a new instance of <see cref="WampClientBuilderFactory{TMessage}"/>.
        /// </summary>
        /// <param name="sessionIdGenerator">The <see cref="IWampIdGenerator"/> used to generate
        /// session ids.</param>
        /// <param name="outgoingSerializer">The <see cref="IWampOutgoingRequestSerializer{TMessage}"/>
        /// used to serialize methods call to <see cref="WampMessage{TMessage}"/>s.</param>
        /// <param name="outgoingHandlerBuilder">The <see cref="IWampOutgoingMessageHandler{TMessage}"/>
        /// used to create the <see cref="IWampOutgoingMessageHandler{TMessage}"/> used to
        /// handle outgoing <see cref="WampMessage{TMessage}"/>s.</param>
        public WampClientBuilderFactory(IWampIdGenerator sessionIdGenerator,
                                        IWampOutgoingRequestSerializer<TMessage> outgoingSerializer,
                                        IWampOutgoingMessageHandlerBuilder<TMessage> outgoingHandlerBuilder)
        {
            mSessionIdGenerator = sessionIdGenerator;
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
        }

        public IWampClientBuilder<TMessage, IWampClient> GetClientBuilder(IWampClientContainer<TMessage, IWampClient> container)
        {
            WampClientBuilder<TMessage> result =
                new WampClientBuilder<TMessage>
                    (mSessionIdGenerator,
                     mOutgoingSerializer,
                     mOutgoingHandlerBuilder,
                     container);

            return result;
        }
    }
}