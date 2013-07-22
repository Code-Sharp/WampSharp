namespace WampSharp.Core.Contracts.V2
{
    public interface IWampClient<TMessage> : IWampAuxiliaryClient<TMessage>,
                                             IWampRpcCaller<TMessage>,
                                             IWampSubscriber<TMessage>
    {
    }

    public interface IWampClient : IWampClient<object>,
                                   IWampAuxiliaryClient,
                                   IWampRpcCaller,
                                   IWampSubscriber
    {
    }
}