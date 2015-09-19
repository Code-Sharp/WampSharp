namespace WampSharp.V2.Rpc
{
    public class WampCalleeAddEventArgs : WampCalleeChangeEventArgs
    {
        public WampCalleeAddEventArgs(IWampRpcOperation operation) : base(operation)
        {
        }
    }
}