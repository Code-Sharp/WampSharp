using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly IWampClientConnectionMonitor mMonitor;

        public WampCaller(IWampServerProxy proxy, IWampFormatter<TMessage> formatter, IWampClientConnectionMonitor monitor)
        {
            mProxy = proxy;
            mFormatter = formatter;
            mMonitor = monitor;

            monitor.ConnectionBroken += OnConnectionBroken;
            monitor.ConnectionError += OnConnectionError;
        }

        public void Invoke(IWampRpcOperationCallback caller,
                           CallOptions options,
                           string procedure)
        {
            RawCallbackAdpater adapter = new RawCallbackAdpater(caller);

            Invoke(adapter, options, procedure);
        }

        public void Invoke(IWampRpcOperationCallback caller,
                           CallOptions options,
                           string procedure,
                           object[] arguments)
        {
            RawCallbackAdpater adapter = new RawCallbackAdpater(caller);

            Invoke(adapter, options, procedure, arguments);
        }

        public void Invoke(IWampRpcOperationCallback caller, CallOptions options, string procedure, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            RawCallbackAdpater adapter = new RawCallbackAdpater(caller);

            Invoke(adapter, options, procedure, arguments, argumentsKeywords);
        }

        public void Invoke(IWampRawRpcOperationCallback caller, CallOptions options, string procedure)
        {
            CallDetails callDetails = new CallDetails(caller, options, procedure);

            long requestId = RegisterCall(callDetails);

            mProxy.Call(requestId, options, procedure);
        }

        public void Invoke(IWampRawRpcOperationCallback caller, CallOptions options, string procedure, object[] arguments)
        {
            CallDetails callDetails = new CallDetails(caller, options, procedure, arguments);

            long requestId = RegisterCall(callDetails);

            mProxy.Call(requestId, options, procedure, arguments);
        }

        public void Invoke(IWampRawRpcOperationCallback caller, CallOptions options, string procedure, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            CallDetails callDetails = new CallDetails(caller, options, procedure, arguments, argumentsKeywords);

            long requestId = RegisterCall(callDetails);

            mProxy.Call(requestId, options, procedure, arguments, argumentsKeywords);
        }

        public void Result(long requestId, TMessage details)
        {
            CallDetails callDetails = TryGetCallDetails(requestId);

            if (callDetails != null)
            {
                callDetails.Caller.Result(this.Formatter, details);
            }
        }

        public void Result(long requestId, TMessage details, TMessage[] arguments)
        {
            CallDetails callDetails = TryGetCallDetails(requestId);

            if (callDetails != null)
            {
                callDetails.Caller.Result(this.Formatter, details, arguments);
            }
        }

        public void Result(long requestId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            CallDetails callDetails = TryGetCallDetails(requestId);

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
            CallDetails result;

            if (mPendingCalls.TryRemove(requestId, out result))
            {
                return result;
            }

            return null;
        }

        private IWampFormatter<TMessage> Formatter
        {
            get
            {
                return mFormatter;
            }
        }

        private class CallDetails
        {
            private readonly IWampRawRpcOperationCallback mCaller;
            private readonly CallOptions mOptions;
            private readonly string mProcedure;
            private readonly object[] mArguments;
            private readonly IDictionary<string, object> mArgumentsKeywords;

            public CallDetails(IWampRawRpcOperationCallback caller, CallOptions options, string procedure, object[] arguments = null, IDictionary<string, object> argumentsKeywords = null)
            {
                mCaller = caller;
                mOptions = options;
                mProcedure = procedure;
                mArguments = arguments;
                mArgumentsKeywords = argumentsKeywords;
            }

            public long RequestId
            {
                get; 
                set;
            }

            public IWampRawRpcOperationCallback Caller
            {
                get
                {
                    return mCaller;
                }
            }

            public CallOptions Options
            {
                get
                {
                    return mOptions;
                }
            }

            public string Procedure
            {
                get
                {
                    return mProcedure;
                }
            }

            public object[] Arguments
            {
                get
                {
                    return mArguments;
                }
            }

            public IDictionary<string, object> ArgumentsKeywords
            {
                get
                {
                    return mArgumentsKeywords;
                }
            }
        }

        private class RawCallbackAdpater : IWampRawRpcOperationCallback
        {
            private readonly IWampRpcOperationCallback mCaller;

            public RawCallbackAdpater(IWampRpcOperationCallback caller)
            {
                mCaller = caller;
            }

            public void Result<T>(IWampFormatter<T> formatter, T details)
            {
                mCaller.Result(details);
            }

            public void Result<T>(IWampFormatter<T> formatter, T details, T[] arguments)
            {
                object[] castedArguments = arguments.Cast<object>().ToArray();
                mCaller.Result(details, castedArguments);
            }

            public void Result<T>(IWampFormatter<T> formatter, T details, T[] arguments, T argumentsKeywords)
            {
                object[] castedArguments = arguments.Cast<object>().ToArray();
                mCaller.Result(details, castedArguments, argumentsKeywords);
            }

            public void Error<T>(IWampFormatter<T> formatter, T details, string error)
            {
                mCaller.Error(details, error);
            }

            public void Error<T>(IWampFormatter<T> formatter, T details, string error, T[] arguments)
            {
                object[] castedArguments = arguments.Cast<object>().ToArray();
                mCaller.Error(details, error, castedArguments);
            }

            public void Error<T>(IWampFormatter<T> formatter, T details, string error, T[] arguments,
                                         T argumentsKeywords)
            {
                object[] castedArguments = arguments.Cast<object>().ToArray();
                mCaller.Error(details, error, castedArguments, argumentsKeywords);
            }
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
        }
    }
}