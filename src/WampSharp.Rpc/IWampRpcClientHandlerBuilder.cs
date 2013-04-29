namespace WampSharp.Rpc
{
    public interface IWampRpcClientHandlerBuilder<TMessage>
    {
        IWampRpcClientHandler<TMessage> Build();
    }
}