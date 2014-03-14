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
    public class WampClientBuilderFactory<TMessage> : IWampClientBuilderFactory<TMessage, IWampClient<TMessage>>
    {
        private readonly IWampIdGenerator mSessionIdGenerator;
        private readonly IWampOutgoingRequestSerializer<TMessage> mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage> mOutgoingHandlerBuilder;
        private IWampBinding<TMessage> mBinding;

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
                                        IWampOutgoingMessageHandlerBuilder<TMessage> outgoingHandlerBuilder, IWampBinding<TMessage> binding)
        {
            mSessionIdGenerator = sessionIdGenerator;
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
            mBinding = binding;
        }

        public IWampClientBuilder<TMessage, IWampClient<TMessage>> GetClientBuilder(IWampClientContainer<TMessage, IWampClient<TMessage>> container)
        {
            WampClientBuilder<TMessage> result =
                new WampClientBuilder<TMessage>
                    (mSessionIdGenerator,
                     mOutgoingSerializer,
                     mOutgoingHandlerBuilder,
                     container,
                     mBinding);

            return result;
        }
    }
}