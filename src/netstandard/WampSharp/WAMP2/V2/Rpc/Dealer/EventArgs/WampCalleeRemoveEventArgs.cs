namespace WampSharp.V2.Rpc
{
    public class WampCalleeRemoveEventArgs : WampCalleeChangeEventArgs
    {
        public WampCalleeRemoveEventArgs(IWampRpcOperation operation) : base(operation)
        {
        }
    }
}