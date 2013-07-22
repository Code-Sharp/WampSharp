using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;

namespace WampSharp.Rpc.Client
{
    public class WampRpcClientHandlerBuilder<TMessage> : IWampRpcClientHandlerBuilder<TMessage>
    {
        private readonly IWampServerProxyFactory<TMessage> mServerProxyFactory;
        private readonly IWampFormatter<TMessage> mFormatter;

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