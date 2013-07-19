using WampSharp.Core.Listener;

namespace WampSharp.Rpc
{
    public interface IWampRpcClientFactory<TMessage>
    {
        TProxy GetClient<TProxy>(IWampConnection<TMessage> connection) where TProxy : class;

        // TODO: Maybe this shouldn't be part of this interface.
        dynamic GetDynamicClient(IWampConnection<TMessage> connection);
    }
}