namespace WampSharp.Core.Contracts
{
    public interface IWampServer : IWampServer<object>
    {
    }

    public interface IWampServer<TMessage> : IWampAuxiliaryServer, IWampRpcServer<TMessage>, IWampRpcPubSubServer<TMessage>
    {
    }
}