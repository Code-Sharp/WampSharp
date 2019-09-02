using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Core.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampServerProxyBuilder{TMessage,TRawClient,TServer}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TRawClient"></typeparam>
    /// <typeparam name="TServer"></typeparam>
    public class WampServerProxyBuilder<TMessage, TRawClient, TServer> : ManualWampServerProxyBuilder<TMessage>
        where TServer : class
    {
        /// <summary>
        /// Creates a new instance of <see cref="WampServerProxyBuilder{TMessage,TRawClient,TServer}"/>
        /// </summary>
        /// <param name="outgoingSerializer">A <see cref="IWampOutgoingRequestSerializer"/>
        /// used in order to serialize requests into <see cref="WampMessage{TMessage}"/>s.</param>
        /// <param name="outgoingHandlerBuilder">A <see cref="IWampServerProxyOutgoingMessageHandlerBuilder{TMessage,TRawClient}"/>
        /// used in order to build an <see cref="IWampOutgoingMessageHandler"/> that will handle serialized
        /// <see cref="WampMessage{TMessage}"/>s.</param>
        public WampServerProxyBuilder(IWampOutgoingRequestSerializer outgoingSerializer,
                                      IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, IWampClient<TMessage>> outgoingHandlerBuilder) :
            base(outgoingSerializer, outgoingHandlerBuilder)
        {
        }
    }
}