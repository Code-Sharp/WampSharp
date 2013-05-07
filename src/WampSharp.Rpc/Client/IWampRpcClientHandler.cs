namespace WampSharp.Rpc
{
    public interface IWampRpcClientHandler
    {
        object Handle(WampRpcCall<object> rpcCall);
    }
}