using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Listener.V1
{
    public class WampClientBuilderFactory<TConnection> : IWampClientBuilderFactory<TConnection, IWampClient>
    {
        private readonly IWampSessionIdGenerator mSessionIdGenerator;
        private readonly IWampOutgoingRequestSerializer<WampMessage<TConnection>> mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TConnection> mOutgoingHandlerBuilder;

        public WampClientBuilderFactory(IWampSessionIdGenerator sessionIdGenerator,
                                        IWampOutgoingRequestSerializer<WampMessage<TConnection>> outgoingSerializer,
                                        IWampOutgoingMessageHandlerBuilder<TConnection> outgoingHandlerBuilder)
        {
            mSessionIdGenerator = sessionIdGenerator;
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
        }

        public IWampClientBuilder<TConnection, IWampClient> GetClientBuilder(IWampClientContainer<TConnection, IWampClient> container)
        {
            WampClientBuilder<TConnection> result =
                new WampClientBuilder<TConnection>
                    (mSessionIdGenerator,
                     mOutgoingSerializer,
                     mOutgoingHandlerBuilder,
                     container);

            return result;
        }
    }
}