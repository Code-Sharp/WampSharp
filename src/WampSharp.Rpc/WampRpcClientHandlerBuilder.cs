using WampSharp.Core.Serialization;

namespace WampSharp.Rpc
{
    public class WampRpcClientHandlerBuilder<TMessage> : IWampRpcClientHandlerBuilder
    {
        private readonly IWampServerProxyFactory<TMessage> mServerProxyFactory;
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampRpcClientHandlerBuilder(IWampFormatter<TMessage> formatter, IWampServerProxyFactory<TMessage> serverProxyFactory)
        {
            mFormatter = formatter;
            mServerProxyFactory = serverProxyFactory;
        }

        public IWampRpcClientHandler<object> Build()
        {
            return new WampRpcClientHandler<TMessage>(mServerProxyFactory, mFormatter);
        }
    }
}