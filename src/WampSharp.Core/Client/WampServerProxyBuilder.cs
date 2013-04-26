using Castle.DynamicProxy;
using WampSharp.Core.Contracts;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Client
{
    public class WampServerProxyBuilder<TMessage> : IWampServerProxyBuilder<TMessage>
    {
        private readonly IWampOutgoingMessageHandlerBuilder<TMessage> mOutgoingHandlerBuilder;
        private readonly ProxyGenerator mProxyGenerator = new ProxyGenerator();
        private readonly IWampOutgoingRequestSerializer<WampMessage<TMessage>> mOutgoingSerializer;

        public WampServerProxyBuilder(IWampOutgoingRequestSerializer<WampMessage<TMessage>> outgoingSerializer,
                                      IWampOutgoingMessageHandlerBuilder<TMessage> outgoingHandlerBuilder)
        {
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
            mOutgoingSerializer = outgoingSerializer;
        }

        public IWampServer Create(IWampConnection<TMessage> connection)
        {
            IWampOutgoingMessageHandler<TMessage> handler = 
                mOutgoingHandlerBuilder.Build(connection);

            WampOutgoingInterceptor<TMessage> interceptor =
                new WampOutgoingInterceptor<TMessage>(mOutgoingSerializer,
                                                      handler);

            WampInterceptorSelector<TMessage> selector =
                new WampInterceptorSelector<TMessage>(interceptor);

            var proxyOptions = new ProxyGenerationOptions() {Selector = selector};

            IWampServer result =
                mProxyGenerator.CreateInterfaceProxyWithoutTarget<IWampServer>
                    (proxyOptions,
                     interceptor);

            return result;
        }
    }
}