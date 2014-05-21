namespace WampSharp.V2.Core.Contracts
{
    public interface IWampClient :
        IWampSessionClient,
        IWampCallee,
        IWampCaller,
        IWampPublisher,
        IWampSubscriber,
        IWampClientProperties
    {
    }

    public interface IWampClient<TMessage> :
        IWampCallee<TMessage>,
        IWampCaller<TMessage>,
        IWampPublisher<TMessage>,
        IWampSubscriber<TMessage>,
        IWampRawClient<TMessage>,
        IWampClientProperties<TMessage>
    {
    }
}