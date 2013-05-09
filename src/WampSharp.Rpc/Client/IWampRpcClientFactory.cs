namespace WampSharp.Rpc
{
    public interface IWampRpcClientFactory
    {
        TProxy GetClient<TProxy>() where TProxy : class;
    }
}