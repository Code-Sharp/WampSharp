using WampSharp.Core.Listener;

namespace WampSharp.Rpc
{
    public interface IWampRpcClientHandlerBuilder<TMessage>
    {
        IWampRpcClientHandler Build(IWampConnection<TMessage> connection);
    }
}