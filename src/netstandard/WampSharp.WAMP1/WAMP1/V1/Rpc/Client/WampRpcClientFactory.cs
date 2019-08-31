﻿using Castle.DynamicProxy;
using WampSharp.Core.Listener;
using WampSharp.Core.Utilities;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// A default implementation of <see cref="IWampRpcClientFactory{TMessage}"/>
    /// using dynamic proxy.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampRpcClientFactory<TMessage> : IWampRpcClientFactory<TMessage>
    {
        private readonly ProxyGenerator mProxyGenerator = CastleDynamicProxyGenerator.Instance;
        private readonly IWampRpcSerializer mSerializer;
        private readonly IWampRpcClientHandlerBuilder<TMessage> mClientHandlerBuilder;

        /// <summary>
        /// Creates a new instance of <see cref="WampRpcClientFactory{TMessage}"/>.
        /// </summary>
        /// <param name="serializer">The <see cref="IWampRpcSerializer"/> used
        /// in order to serialize RPC calls.</param>
        /// <param name="clientHandlerBuilder">The <see cref="IWampRpcClientHandlerBuilder{TMessage}"/>
        /// used in order to build the handler that handles the <see cref="WampRpcCall"/>s.</param>
        public WampRpcClientFactory(IWampRpcSerializer serializer, IWampRpcClientHandlerBuilder<TMessage> clientHandlerBuilder)
        {
            mSerializer = serializer;
            mClientHandlerBuilder = clientHandlerBuilder;
        }

        public TProxy GetClient<TProxy>(IWampConnection<TMessage> connection) where TProxy : class
        {
            IWampRpcClientHandler handler = mClientHandlerBuilder.Build(connection);

            WampRpcClientSyncInterceptor syncInterceptor =
                new WampRpcClientSyncInterceptor(mSerializer, handler);

            WampRpcClientAsyncInterceptor asyncInterceptor =
                new WampRpcClientAsyncInterceptor(mSerializer, handler);

            ProxyGenerationOptions generationOptions =
                new ProxyGenerationOptions {Selector = new WampRpcClientInterceptorSelector()};

            TProxy result =
                mProxyGenerator.CreateInterfaceProxyWithoutTarget<TProxy>
                    (generationOptions,
                     syncInterceptor,
                     asyncInterceptor);

            return result;
        }

        public dynamic GetDynamicClient(IWampConnection<TMessage> connection)
        {
            IWampRpcClientHandler handler = mClientHandlerBuilder.Build(connection);

            DynamicWampRpcClient client = new DynamicWampRpcClient(handler, mSerializer);

            return client;
        }
    }
}