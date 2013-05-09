namespace WampSharp.Core.Contracts.V2
{
    public interface IWampServer<TMessage> : IWampAuxiliaryServer<TMessage>,
                                             IWampRpcCallee<TMessage>,
                                             IWampBroker<TMessage>
    {
    }
}