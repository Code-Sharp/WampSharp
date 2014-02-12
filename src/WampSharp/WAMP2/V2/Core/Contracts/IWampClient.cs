namespace WampSharp.V2.Core.Contracts
{
    public interface IWampClient : IWampSessionManagementClient, IWampCallee,
                                    IWampCaller
    {
        long Session { get; }
    }
}