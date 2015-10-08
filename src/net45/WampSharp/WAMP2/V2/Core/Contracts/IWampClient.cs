namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Contains all WAMPv2 client messages.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampClient<TMessage> : IWampSessionClient,
        IWampError<TMessage>,
        IWampCallee<TMessage>,
        IWampCaller<TMessage>,
        IWampPublisher<TMessage>,
        IWampSubscriber<TMessage>
    {
    }
}