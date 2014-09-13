using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class WampRpcOperationCallback : IWampClientRawRpcOperationCallback,
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

        public void Result<TResult>(IWampFormatter<TResult> formatter, ResultDetails details)
        {
            mCaller.Result(mRequestId, details);
        }

        public void Result<TResult>(IWampFormatter<TResult> formatter, ResultDetails details, TResult[] arguments)
        {
            mCaller.Result(mRequestId, details, arguments.Cast<object>().ToArray());
        }

        public void Result<TResult>(IWampFormatter<TResult> formatter, ResultDetails details, TResult[] arguments, IDictionary<string, TResult> argumentsKeywords)
        {
            mCaller.Result(mRequestId, details, arguments.Cast<object>().ToArray(), argumentsKeywords.ToDictionary(x => x.Key, x => (object)x.Value));
        }

        public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error)
        {
            mCaller.CallError(mRequestId, details, error);
        }

        public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error, TResult[] arguments)
        {
            mCaller.CallError(mRequestId, details, error, arguments.Cast<object>().ToArray());
        }

        public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error, TResult[] arguments, TResult argumentsKeywords)
        {
            mCaller.CallError(mRequestId, details, error, arguments.Cast<object>().ToArray(), argumentsKeywords);
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