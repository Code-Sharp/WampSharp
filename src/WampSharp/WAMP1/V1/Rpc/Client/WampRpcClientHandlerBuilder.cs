using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampRpcClientHandlerBuilder{TMessage}"/>
    /// using <see cref="WampRpcClientHandler{TMessage}"/>
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampRpcClientHandlerBuilder<TMessage> : IWampRpcClientHandlerBuilder<TMessage>
    {
        private readonly IWampServerProxyFactory<TMessage> mServerProxyFactory;
        private readonly IWampFormatter<TMessage> mFormatter;

        /// <summary>
        /// Creates a new instance of <see cref="WampRpcClientHandler{TMessage}"/>.
        /// </summary>
        public WampRpcClientHandlerBuilder(IWampFormatter<TMessage> formatter, IWampServerProxyFactory<TMessage> serverProxyFactory)
        {
            mFormatter = formatter;
            mServerProxyFactory = serverProxyFactory;
        }

        public IWampRpcClientHandler Build(IWampConnection<TMessage> connection)
        {
            return new WampRpcClientHandler<TMessage>(mServerProxyFactory, connection, mFormatter);
        }
    }
}