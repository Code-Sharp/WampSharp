using Castle.DynamicProxy;
using WampSharp.Core.Contracts;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Curie;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Listener
{
    public class WampClientBuilder<TMessage> : IWampClientBuilder<TMessage>
    {
        private readonly IWampClientContainer<TMessage> mContainer;
        private readonly ProxyGenerator mGenerator = new ProxyGenerator();
        private readonly IWampOutgoingRequestSerializer<WampMessage<TMessage>> mOutgoingSerializer;
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage> mOutgoingHandlerBuilder;
        private readonly IWampSessionIdGenerator mSessionIdGenerator;

        public WampClientBuilder(IWampSessionIdGenerator sessionIdGenerator, IWampOutgoingRequestSerializer<WampMessage<TMessage>> outgoingSerializer, IWampOutgoingMessageHandlerBuilder<TMessage> outgoingHandlerBuilder, IWampClientContainer<TMessage> container)
        {
            mOutgoingSerializer = outgoingSerializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
            mContainer = container;
            mSessionIdGenerator = sessionIdGenerator;
        }

        public IWampClient Create(IWampConnection<TMessage> connection)
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
                (new WampClientContainerDisposable<TMessage>
                    (mContainer, connection));

            proxyGenerationOptions.AddMixinInstance(new WampCurieMapper());

            IWampClient result =
                mGenerator.CreateInterfaceProxyWithoutTarget<IWampClient>
                    (proxyGenerationOptions, wampOutgoingInterceptor,
                    new SessionIdPropertyInterceptor(mSessionIdGenerator.Generate()));

            return result;
        }
    }
}