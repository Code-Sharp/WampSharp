using System;
using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Core.Listener.ClientBuilder;

namespace WampSharp.V1.Core.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampServerProxyBuilder{TMessage,TRawClient,TServer}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TRawClient"></typeparam>
    public class ManualWampServerProxyBuilder<TMessage, TRawClient> : IWampServerProxyBuilder<TMessage, TRawClient, IWampServer>
    {
        private readonly IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient> mOutgoingHandlerBuilder;
        private readonly IWampOutgoingRequestSerializer mOutgoingSerializer;

        /// <summary>
        /// Creates a new instance of <see cref="ManualWampServerProxyBuilder{TMessage, TClient}"/>
        /// </summary>
        /// <param name="outgoingSerializer">A <see cref="IWampOutgoingRequestSerializer"/>
        /// used in order to serialize requests into <see cref="WampMessage{TMessage}"/>s.</param>
        /// <param name="outgoingHandlerBuilder">A <see cref="IWampServerProxyOutgoingMessageHandlerBuilder{TMessage,TRawClient}"/>
        /// used in order to build an <see cref="IWampOutgoingMessageHandler"/> that will handle serialized
        /// <see cref="WampMessage{TMessage}"/>s.</param>
        public ManualWampServerProxyBuilder(IWampOutgoingRequestSerializer outgoingSerializer,
                                      IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient> outgoingHandlerBuilder)
        {
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
            mOutgoingSerializer = outgoingSerializer;
        }

        public IWampServer Create(TRawClient client, IWampConnection<TMessage> connection)
        {
            IWampOutgoingMessageHandler handler =
                mOutgoingHandlerBuilder.Build(client, connection);

            IDisposable disposable = new DisposableForwarder(connection);

            WampServerProxy result = new WampServerProxy(handler, mOutgoingSerializer, disposable);

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