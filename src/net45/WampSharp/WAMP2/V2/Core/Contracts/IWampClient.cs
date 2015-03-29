namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents a WAMP2 client proxy.
    /// </summary>
    public interface IWampClient :
        IWampSessionClient,
        IWampCallee,
        IWampCaller,
        IWampPublisher,
        IWampSubscriber,
        IWampClientProperties
    {
    }

    /// <summary>
    /// Represents a WAMP2 client/client proxy.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampClient<TMessage> :
        IWampSessionClient,
        IWampError<TMessage>,
        IWampCallee<TMessage>,
        IWampCaller<TMessage>,
        IWampPublisher<TMessage>,
        IWampSubscriber<TMessage>,
        IWampRawClient<TMessage>,
        IWampClientProperties<TMessage>
    {
    }
}