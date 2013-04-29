namespace WampSharp.Rpc
{
    public interface IWampRpcClientHandler<TMessage>
    {
        object Handle(WampRpcCall<TMessage> rpcCall);
    }

    public interface IWampRpcClientHandler : IWampRpcClientHandler<object>
    {
    }
}