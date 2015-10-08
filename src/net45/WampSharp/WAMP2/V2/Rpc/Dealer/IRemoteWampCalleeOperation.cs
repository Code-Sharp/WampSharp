namespace WampSharp.V2.Rpc
{
    public interface IRemoteWampCalleeOperation : IWampRpcOperation
    {
        long SessionId { get; }         
    }
}