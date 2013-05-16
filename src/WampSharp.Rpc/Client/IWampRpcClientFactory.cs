namespace WampSharp.Rpc
{
    public interface IWampRpcClientFactory
    {
        TProxy GetClient<TProxy>() where TProxy : class;

        // TODO: Maybe this shouldn't be part of this interface.
        dynamic GetDynamicClient();
    }
}