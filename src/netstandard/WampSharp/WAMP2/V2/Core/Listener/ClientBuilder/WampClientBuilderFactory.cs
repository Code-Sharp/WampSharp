using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core.Listener.ClientBuilder
{
    /// <summary>
    /// An implementation of <see cref="IWampClientBuilderFactory{TMessage,TClient}"/>
    /// using <see cref="WampClientBuilder{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampClientBuilderFactory<TMessage> : IWampClientBuilderFactory<TMessage, IWampClientProxy<TMessage>>
    {
        private readonly IWampOutgoingRequestSerializer mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage> mOutgoingHandlerBuilder;
        private readonly IWampBinding<TMessage> mBinding;

        /// <summary>
        /// Creates a new instance of <see cref="WampClientBuilderFactory{TMessage}"/>.
        /// </summary>
        /// <param name="outgoingSerializer">The <see cref="IWampOutgoingRequestSerializer"/>
        /// used to serialize methods call to <see cref="WampMessage{TMessage}"/>s.</param>
        /// <param name="outgoingHandlerBuilder">The <see cref="IWampOutgoingMessageHandler"/>
        /// used to create the <see cref="IWampOutgoingMessageHandler"/> used to
        /// handle outgoing <see cref="WampMessage{TMessage}"/>s.</param>
        /// <param name="binding">The <see cref="IWampBinding{TMessage}"/> to use.</param>
        public WampClientBuilderFactory(IWampOutgoingRequestSerializer outgoingSerializer,
                                        IWampOutgoingMessageHandlerBuilder<TMessage> outgoingHandlerBuilder, IWampBinding<TMessage> binding)
        {
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
            mBinding = binding;
        }

        public IWampClientBuilder<TMessage, IWampClientProxy<TMessage>> GetClientBuilder(IWampClientContainer<TMessage, IWampClientProxy<TMessage>> container)
        {
            WampClientBuilder<TMessage> result =
                new WampClientBuilder<TMessage>
                    (mOutgoingSerializer,
                     mOutgoingHandlerBuilder,
                     container,
                     mBinding);

            return result;
        }
    }
}