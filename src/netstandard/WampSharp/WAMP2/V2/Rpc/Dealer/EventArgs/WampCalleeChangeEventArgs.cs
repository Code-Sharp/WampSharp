using System;

namespace WampSharp.V2.Rpc
{
    public class WampCalleeChangeEventArgs : EventArgs
    {
        public WampCalleeChangeEventArgs(IWampRpcOperation operation)
        {
            Operation = operation;
        }

        public IWampRpcOperation Operation { get; }
    }
}