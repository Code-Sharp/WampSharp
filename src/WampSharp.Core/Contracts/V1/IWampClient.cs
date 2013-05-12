namespace WampSharp.Core.Contracts.V1
{
    public interface IWampClient : IWampClient<object>,
                                   IWampAuxiliaryClient,
                                   IWampRpcClient,
                                   IWampPubSubClient
    {
    }

    public interface IWampClient<TMessage> : IWampAuxiliaryClient,
                                             IWampRpcClient<TMessage>,
                                             IWampPubSubClient<TMessage>
    {
    }
}