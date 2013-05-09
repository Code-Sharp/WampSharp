using Castle.DynamicProxy;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Client
{
    public class WampServerProxyBuilder<TMessage, TRawClient, TServer> : IWampServerProxyBuilder<TMessage, TRawClient, TServer>
        where TServer : class
    {
        private readonly IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient> mOutgoingHandlerBuilder;
        private readonly ProxyGenerator mProxyGenerator = new ProxyGenerator();
        private readonly IWampOutgoingRequestSerializer<WampMessage<TMessage>> mOutgoingSerializer;

        public WampServerProxyBuilder(IWampOutgoingRequestSerializer<WampMessage<TMessage>> outgoingSerializer,
                                      IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient> outgoingHandlerBuilder)
        {
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
            mOutgoingSerializer = outgoingSerializer;
        }

        public TServer Create(TRawClient client, IWampConnection<TMessage> connection)
        {
            IWampOutgoingMessageHandler<TMessage> handler =
                mOutgoingHandlerBuilder.Build(client, connection);

            WampOutgoingInterceptor<TMessage> interceptor =
                new WampOutgoingInterceptor<TMessage>(mOutgoingSerializer,
                                                      handler);

            WampInterceptorSelector<TMessage> selector =
                new WampInterceptorSelector<TMessage>(interceptor);

            var proxyOptions = new ProxyGenerationOptions() { Selector = selector };

            TServer result =
                mProxyGenerator.CreateInterfaceProxyWithoutTarget<TServer>
                    (proxyOptions,
                     interceptor);

            return result;
        }
    }
}