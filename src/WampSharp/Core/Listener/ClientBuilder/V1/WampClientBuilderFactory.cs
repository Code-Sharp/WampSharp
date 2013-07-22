using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Listener.V1
{
    /// <summary>
    /// An implementation of <see cref="IWampClientBuilderFactory{TMessage,TClient}"/>
    /// using <see cref="WampClientBuilder{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampClientBuilderFactory<TMessage> : IWampClientBuilderFactory<TMessage, IWampClient>
    {
        private readonly IWampSessionIdGenerator mSessionIdGenerator;
        private readonly IWampOutgoingRequestSerializer<TMessage> mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage> mOutgoingHandlerBuilder;

        public WampClientBuilderFactory(IWampSessionIdGenerator sessionIdGenerator,
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