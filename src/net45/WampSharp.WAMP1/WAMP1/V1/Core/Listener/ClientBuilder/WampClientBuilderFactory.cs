using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Core.Listener.ClientBuilder
{
    /// <summary>
    /// An implementation of <see cref="IWampClientBuilderFactory{TMessage,TClient}"/>
    /// using <see cref="WampClientBuilder{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampClientBuilderFactory<TMessage> : IWampClientBuilderFactory<TMessage, IWampClient>
    {
        private readonly IWampOutgoingRequestSerializer mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage> mOutgoingHandlerBuilder;

        /// <summary>
        /// Creates a new instance of <see cref="WampClientBuilderFactory{TMessage}"/>.
        /// </summary>
        /// <param name="outgoingSerializer">The <see cref="IWampOutgoingRequestSerializer"/>
        /// used to serialize methods call to <see cref="WampMessage{TMessage}"/>s.</param>
        /// <param name="outgoingHandlerBuilder">The <see cref="IWampOutgoingMessageHandler"/>
        /// used to create the <see cref="IWampOutgoingMessageHandler"/> used to
        /// handle outgoing <see cref="WampMessage{TMessage}"/>s.</param>
        public WampClientBuilderFactory(IWampOutgoingRequestSerializer outgoingSerializer,
                                        IWampOutgoingMessageHandlerBuilder<TMessage> outgoingHandlerBuilder)
        {
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
        }

        public IWampClientBuilder<TMessage, IWampClient> GetClientBuilder(IWampClientContainer<TMessage, IWampClient> container)
        {
            WampClientBuilder<TMessage> result =
                new WampClientBuilder<TMessage>
                    (mOutgoingSerializer,
                     mOutgoingHandlerBuilder,
                     container);

            return result;
        }
    }
}