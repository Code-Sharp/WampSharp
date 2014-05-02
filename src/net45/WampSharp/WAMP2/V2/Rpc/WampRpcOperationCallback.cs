using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcOperationCallback : IWampRpcOperationCallback,
        ICallbackDisconnectionNotifier
    {
        private readonly IWampCaller mCaller;
        private readonly long mRequestId;

        public WampRpcOperationCallback(IWampCaller caller, long requestId)
        {
            mCaller = caller;
            mRequestId = requestId;

            IWampConnectionMonitor monitor = caller as IWampConnectionMonitor;
            monitor.ConnectionClosed += OnConnectionClosed;
        }

        public void Result(object details)
        {
            mCaller.Result(mRequestId, details);
        }

        public void Result(object details, object[] arguments)
        {
            mCaller.Result(mRequestId, details, arguments);
        }

        public void Result(object details, object[] arguments, object argumentsKeywords)
        {
            mCaller.Result(mRequestId, details, arguments, argumentsKeywords);
        }

        public void Error(object details, string error)
        {
            mCaller.CallError(mRequestId, details, error);
        }

        public void Error(object details, string error, object[] arguments)
        {
            mCaller.CallError(mRequestId, details, error, arguments);
        }

        public void Error(object details, string error, object[] arguments, object argumentsKeywords)
        {
            mCaller.CallError(mRequestId, details, error, arguments, argumentsKeywords);
        }

        public event EventHandler Disconnected;

        private void OnConnectionClosed(object sender, EventArgs e)
        {
            IWampConnectionMonitor monitor = sender as IWampConnectionMonitor;
            monitor.ConnectionClosed -= OnConnectionClosed;

            RaiseDisconnected();
        }

        protected virtual void RaiseDisconnected()
        {
            EventHandler handler = Disconnected;
            
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #region Equality members

        protected bool Equals(WampRpcOperationCallback other)
        {
            return Equals(mCaller, other.mCaller);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((WampRpcOperationCallback)obj);
        }

        public override int GetHashCode()
        {
            if (mCaller != null)
            {
                return mCaller.GetHashCode();
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}