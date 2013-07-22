using WampSharp.Core.Listener;

namespace WampSharp.Rpc.Client
{
    public interface IWampRpcClientHandlerBuilder<TMessage>
    {
        IWampRpcClientHandler Build(IWampConnection<TMessage> connection);
    }
}