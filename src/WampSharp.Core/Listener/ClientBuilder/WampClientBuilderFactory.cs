using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Listener
{
    public class WampClientBuilderFactory<TConnection> : IWampClientBuilderFactory<TConnection>
    {
        private readonly IWampSessionIdGenerator mSessionIdGenerator;
        private readonly IWampOutgoingRequestSerializer<WampMessage<TConnection>> mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TConnection> mOutgoingHandlerBuilder;
        private WampClientBuilder<TConnection> mLastClientBuilder;

        public WampClientBuilderFactory(IWampSessionIdGenerator sessionIdGenerator,
                                        IWampOutgoingRequestSerializer<WampMessage<TConnection>> outgoingSerializer,
                                        IWampOutgoingMessageHandlerBuilder<TConnection> outgoingHandlerBuilder)
        {
            mSessionIdGenerator = sessionIdGenerator;
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
        }

        public WampClientBuilder<TConnection> LastClientBuilder
        {
            get
            {
                return mLastClientBuilder;
            }
        }

        public IWampClientBuilder<TConnection> GetClientBuilder(IWampClientContainer<TConnection> container)
        {
            mLastClientBuilder = new WampClientBuilder<TConnection>
                (mSessionIdGenerator,
                 mOutgoingSerializer,
                 mOutgoingHandlerBuilder,
                 container);

            return LastClientBuilder;
        }

    }
}