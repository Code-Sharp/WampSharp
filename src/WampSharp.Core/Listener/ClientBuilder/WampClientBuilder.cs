using Castle.DynamicProxy;
using WampSharp.Core.Contracts;
using WampSharp.Core.Curie;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Listener
{
    public class WampClientBuilder<TMessage, TConnection> : IWampClientBuilder<TConnection>
    {
        private IWampClientContainer<TConnection> mContainer;
        private readonly ProxyGenerator mGenerator = new ProxyGenerator();
        private readonly IWampOutgoingRequestSerializer<WampMessage<TMessage>> mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage, TConnection> mOutgoingHandlerBuilder;

        public WampClientBuilder(IWampOutgoingRequestSerializer<WampMessage<TMessage>> outgoingSerializer, IWampMessageFormatter<TMessage> wampMessageFormatter,
            IWampOutgoingMessageHandlerBuilder<TMessage, TConnection> outgoingHandlerBuilder)
        {
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
        }

        public IWampClientContainer<TConnection> Container
        {
            get
            {
                return mContainer;
            }
            set
            {
                mContainer = value;
            }
        }

        public IWampClient Create(TConnection connection)
        {
            WampOutgoingInterceptor<TMessage> wampOutgoingInterceptor =
                new WampOutgoingInterceptor<TMessage>
                    (mOutgoingSerializer,
                     mOutgoingHandlerBuilder.Build(connection));

            ProxyGenerationOptions proxyGenerationOptions =
                new ProxyGenerationOptions()
                    {
                        Selector =
                            new WampInterceptorSelector<TMessage>
                            (wampOutgoingInterceptor)
                    };

            proxyGenerationOptions.AddMixinInstance
                (new WampClientContainerDisposable<TConnection>(Container, connection));

            proxyGenerationOptions.AddMixinInstance(new WampCurieMapper());

            IWampClient result =
                mGenerator.CreateInterfaceProxyWithoutTarget<IWampClient>
                    (proxyGenerationOptions, wampOutgoingInterceptor);

            return result;
        }
    }
}