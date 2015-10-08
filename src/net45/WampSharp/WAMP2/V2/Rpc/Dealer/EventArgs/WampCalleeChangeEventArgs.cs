using System;

namespace WampSharp.V2.Rpc
{
    public class WampCalleeChangeEventArgs : EventArgs
    {
        private readonly IWampRpcOperation mOperation;

        public WampCalleeChangeEventArgs(IWampRpcOperation operation)
        {
            mOperation = operation;
        }

        public IWampRpcOperation Operation
        {
            get
            {
                return mOperation;
            }
        }
    }
}