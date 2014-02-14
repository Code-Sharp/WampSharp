namespace WampSharp.V2.Core.Contracts
{
    public interface IWampClient :
        IWampSessionClient,
        IWampCallee,
        IWampCaller,
        IWampPublisher,
        IWampSubscriber
    {
        long Session { get; }
    }
}