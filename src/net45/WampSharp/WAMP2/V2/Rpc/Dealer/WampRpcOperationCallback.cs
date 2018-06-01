using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class WampRpcOperationCallback : IWampRawRpcOperationClientCallback,
        IWampRawRpcOperationRouterCallback,
        ICallbackDisconnectionNotifier
    {
        public IWampCaller Caller { get; }
        public long Session { get; }
        public long RequestId { get; }

        private readonly IWampConnectionMonitor mMonitor;

        public WampRpcOperationCallback(IWampCaller caller, long requestId)
        {
            Caller = caller;
            Session = ((IWampClientProperties) caller).Session;
            RequestId = requestId;

            mMonitor = caller as IWampConnectionMonitor;
            mMonitor.ConnectionClosed += OnConnectionClosed;
        }

        public void Result<TResult>(IWampFormatter<TResult> formatter, ResultDetails details)
        {
            Caller.Result(RequestId, details);
        }

        public void Result<TResult>(IWampFormatter<TResult> formatter, ResultDetails details, TResult[] arguments)
        {
            Caller.Result(RequestId, details, arguments.Cast<object>().ToArray());
        }

        public void Result<TResult>(IWampFormatter<TResult> formatter, ResultDetails details, TResult[] arguments, IDictionary<string, TResult> argumentsKeywords)
        {
            Caller.Result(RequestId, details, arguments.Cast<object>().ToArray(), argumentsKeywords.ToDictionary(x => x.Key, x => (object)x.Value));
        }

        private ResultDetails GetResultDetails(YieldOptions details)
        {
            return new ResultDetails { Progress = details.Progress };
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, YieldOptions details)
        {
            ResultDetails resultDetails = GetResultDetails(details);
            this.Result(formatter, resultDetails);
            UnregisterConnectionClosedIfNeeded(details);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, YieldOptions details, TMessage[] arguments)
        {
            ResultDetails resultDetails = GetResultDetails(details);
            this.Result(formatter, resultDetails, arguments);
            UnregisterConnectionClosedIfNeeded(details);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, YieldOptions details, TMessage[] arguments,
                                     IDictionary<string, TMessage> argumentsKeywords)
        {
            ResultDetails resultDetails = GetResultDetails(details);
            this.Result(formatter, resultDetails, arguments, argumentsKeywords);
            UnregisterConnectionClosedIfNeeded(details);
        }

        private void UnregisterConnectionClosedIfNeeded(YieldOptions details)
        {
            if (details.Progress != true)
            {
                mMonitor.ConnectionClosed -= OnConnectionClosed;
            }
        }

        public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error)
        {
            Caller.CallError(RequestId, details, error);
            UnregisterConnectionClosed();
        }

        public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error, TResult[] arguments)
        {
            Caller.CallError(RequestId, details, error, arguments.Cast<object>().ToArray());
            UnregisterConnectionClosed();
        }

        public void Error<TResult>(IWampFormatter<TResult> formatter, TResult details, string error, TResult[] arguments, TResult argumentsKeywords)
        {
            Caller.CallError(RequestId, details, error, arguments.Cast<object>().ToArray(), argumentsKeywords);
            UnregisterConnectionClosed();
        }

        private void UnregisterConnectionClosed()
        {
            mMonitor.ConnectionClosed -= OnConnectionClosed;
        }

        public event EventHandler Disconnected;

        private void OnConnectionClosed(object sender, EventArgs e)
        {
            mMonitor.ConnectionClosed -= OnConnectionClosed;
            RaiseDisconnected();
        }

        private void RaiseDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        #region Equality Members

        protected bool Equals(WampRpcOperationCallback other)
        {
            return Session == other.Session && RequestId == other.RequestId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WampRpcOperationCallback) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Session.GetHashCode() * 397) ^ RequestId.GetHashCode();
            }
        }

        #endregion
    }
}