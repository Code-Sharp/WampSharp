namespace WampSharp.Rpc
{
    public interface IWampRpcClientHandlerBuilder : IWampRpcClientHandlerBuilder<object>
    {
    }

    public interface IWampRpcClientHandlerBuilder<TMessage>
    {
        IWampRpcClientHandler<TMessage> Build();
    }
}