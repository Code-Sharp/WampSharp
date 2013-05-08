namespace WampSharp.Core.Contracts.V1
{
    public interface IWampServer : IWampServer<object>
    {
    }

    public interface IWampServer<TMessage> : IWampAuxiliaryServer, IWampRpcServer<TMessage>, IWampRpcPubSubServer<TMessage>
    {
    }
}