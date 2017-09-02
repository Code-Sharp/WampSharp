#if CASTLE
using System;
using Castle.DynamicProxy;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Core.Utilities;

namespace WampSharp.Core.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampServerProxyBuilder{TMessage,TRawClient,TServer}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TRawClient"></typeparam>
    /// <typeparam name="TServer"></typeparam>
    public class WampServerProxyBuilder<TMessage, TRawClient, TServer> : IWampServerProxyBuilder<TMessage, TRawClient, TServer>
        where TServer : class
    {
        private readonly IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient> mOutgoingHandlerBuilder;
        private readonly ProxyGenerator mProxyGenerator = CastleDynamicProxyGenerator.Instance;
        private readonly IWampOutgoingRequestSerializer mOutgoingSerializer;

        /// <summary>
        /// Creates a new instance of <see cref="WampServerProxyBuilder{TMessage,TRawClient,TServer}"/>
        /// </summary>
        /// <param name="outgoingSerializer">A <see cref="IWampOutgoingRequestSerializer"/>
        /// used in order to serialize requests into <see cref="WampMessage{TMessage}"/>s.</param>
        /// <param name="outgoingHandlerBuilder">A <see cref="IWampServerProxyOutgoingMessageHandlerBuilder{TMessage,TRawClient}"/>
        /// used in order to build an <see cref="IWampOutgoingMessageHandler"/> that will handle serialized
        /// <see cref="WampMessage{TMessage}"/>s.</param>
        public WampServerProxyBuilder(IWampOutgoingRequestSerializer outgoingSerializer,
                                      IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient> outgoingHandlerBuilder)
        {
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
            mOutgoingSerializer = outgoingSerializer;
        }

        public TServer Create(TRawClient client, IWampConnection<TMessage> connection)
        {
            IWampOutgoingMessageHandler handler =
                mOutgoingHandlerBuilder.Build(client, connection);

            WampOutgoingInterceptor<TMessage> interceptor =
                new WampOutgoingInterceptor<TMessage>(mOutgoingSerializer,
                                                      handler);

            var proxyOptions = new ProxyGenerationOptions();

            proxyOptions.AddMixinInstance(new DisposableForwarder(connection));

            TServer result =
                mProxyGenerator.CreateInterfaceProxyWithoutTarget<TServer>
                    (proxyOptions,
                     interceptor);

            return result;
        }

        private class DisposableForwarder : IDisposable
        {
            private readonly IDisposable mDisposable;

            public DisposableForwarder(IDisposable disposable)
            {
                mDisposable = disposable;
            }

            public void Dispose()
            {
                mDisposable.Dispose();
            }
        }
    }
}
#endif