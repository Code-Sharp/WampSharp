using System.Collections.Generic;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    internal class WampCaller<TMessage> : 
        IWampRpcOperationInvokerProxy, IWampCaller<TMessage>,
        IWampCallerError<TMessage>
    {
        private readonly IWampServerProxy mProxy;
        private readonly WampIdMapper<CallDetails> mPendingCalls = new WampIdMapper<CallDetails>();
        private readonly IWampClientConnectionMonitor mMonitor;

        public WampCaller(IWampServerProxy proxy, IWampFormatter<TMessage> formatter, IWampClientConnectionMonitor monitor)
        {
            mProxy = proxy;
            Formatter = formatter;
            mMonitor = monitor;
            monitor.ConnectionBroken += OnConnectionBroken;
            monitor.ConnectionError += OnConnectionError;
        }

        public IWampCancellableInvocationProxy Invoke(IWampRawRpcOperationClientCallback caller, CallOptions options, string procedure)
        {
            if (!IsConnected)
            {
                throw new WampSessionNotEstablishedException();
            }

            CallDetails callDetails = new CallDetails(caller, options, procedure);

            long requestId = RegisterCall(callDetails);

            mProxy.Call(requestId, options, procedure);

            return new WampCancellableInvocationProxy(mProxy, requestId);
        }

        public IWampCancellableInvocationProxy Invoke(IWampRawRpcOperationClientCallback caller, CallOptions options, string procedure, object[] arguments)
        {
            if (!IsConnected)
            {
                throw new WampSessionNotEstablishedException();
            }

            CallDetails callDetails = new CallDetails(caller, options, procedure, arguments);

            long requestId = RegisterCall(callDetails);

            mProxy.Call(requestId, options, procedure, arguments);

            return new WampCancellableInvocationProxy(mProxy, requestId);
        }

        public IWampCancellableInvocationProxy Invoke(IWampRawRpcOperationClientCallback caller, CallOptions options, string procedure, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            if (!IsConnected)
            {
                throw new WampSessionNotEstablishedException();
            }

            CallDetails callDetails = new CallDetails(caller, options, procedure, arguments, argumentsKeywords);

            long requestId = RegisterCall(callDetails);

            mProxy.Call(requestId, options, procedure, arguments, argumentsKeywords);

            return new WampCancellableInvocationProxy(mProxy, requestId);
        }

        private bool IsConnected => mMonitor.IsConnected;

        public void Result(long requestId, ResultDetails details)
        {
            CallDetails callDetails = TryGetCallDetails(requestId, details);

            if (callDetails != null)
            {
                callDetails.Caller.Result(this.Formatter, details);
            }
        }

        public void Result(long requestId, ResultDetails details, TMessage[] arguments)
        {
            CallDetails callDetails = TryGetCallDetails(requestId, details);

            if (callDetails != null)
            {
                callDetails.Caller.Result(this.Formatter, details, arguments);
            }
        }

        public void Result(long requestId, ResultDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            CallDetails callDetails = TryGetCallDetails(requestId, details);

            if (callDetails != null)
            {
                callDetails.Caller.Result(this.Formatter, details, arguments, argumentsKeywords);
            }
        }

        public void CallError(long requestId, TMessage details, string error)
        {
            CallDetails callDetails = TryGetCallDetails(requestId);

            if (callDetails != null)
            {
                callDetails.Caller.Error(this.Formatter, details, error);
            }
        }

        public void CallError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            CallDetails callDetails = TryGetCallDetails(requestId);

            if (callDetails != null)
            {
                callDetails.Caller.Error(this.Formatter, details, error, arguments);
            }
        }

        public void CallError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            CallDetails callDetails = TryGetCallDetails(requestId);

            if (callDetails != null)
            {
                callDetails.Caller.Error(this.Formatter, details, error, arguments, argumentsKeywords);
            }
        }

        private long RegisterCall(CallDetails callDetails)
        {
            long requestId = mPendingCalls.Add(callDetails);
            callDetails.RequestId = requestId;
            return requestId;
        }

        private CallDetails TryGetCallDetails(long requestId)
        {

            if (mPendingCalls.TryRemove(requestId, out CallDetails result))
            {
                return result;
            }

            return null;
        }

        private CallDetails TryGetCallDetails(long requestId, ResultDetails details)
        {

            if (mPendingCalls.TryGetValue(requestId, out CallDetails result))
            {
                bool progressive = details.Progress == true;

                if (!progressive)
                {
                    mPendingCalls.TryRemove(requestId, out result);
                }

                return result;
            }

            return null;
        }

        private IWampFormatter<TMessage> Formatter { get; }

        private class CallDetails
        {
            private readonly IDictionary<string, object> mArgumentsKeywords;

            public CallDetails(IWampRawRpcOperationClientCallback caller, CallOptions options, string procedure, object[] arguments = null, IDictionary<string, object> argumentsKeywords = null)
            {
                Caller = caller;
                Options = options;
                Procedure = procedure;
                Arguments = arguments;
                mArgumentsKeywords = argumentsKeywords;
            }

            public long RequestId
            {
                get; 
                set;
            }

            public IWampRawRpcOperationClientCallback Caller { get; }

            public CallOptions Options { get; }

            public string Procedure { get; }

            public object[] Arguments { get; }

            public IDictionary<string, object> ArgumentsKeywords => mArgumentsKeywords;
        }

        public void OnConnectionError(object sender, WampConnectionErrorEventArgs wampConnectionErrorEventArgs)
        {
            Cleanup();
        }

        public void OnConnectionBroken(object sender, WampSessionCloseEventArgs wampSessionCloseEventArgs)
        {
            Cleanup();
        }

        private void Cleanup()
        {
            // TODO: Just cleanup structures.
            // TODO: Services forward errors to client
            mPendingCalls.Clear();
        }
    }
}