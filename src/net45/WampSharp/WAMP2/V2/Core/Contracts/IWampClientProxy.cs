namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents a WAMP2 client proxy.
    /// </summary>
    public interface IWampClientProxy :
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
    public interface IWampClientProxy<TMessage> :
        IWampClientProxy,
        IWampClientProperties<TMessage>,
        IWampRawClient<TMessage>
    {
    }
}