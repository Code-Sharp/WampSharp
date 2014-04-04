using System;
using System.Linq;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    internal class WampCaller<TMessage> : IWampRpcOperationInvokerProxy, IWampCaller<TMessage>
    {
        private readonly IWampServerProxy mProxy;
        private readonly WampIdMapper<CallDetails> mPendingCalls = new WampIdMapper<CallDetails>();

        public WampCaller(IWampServerProxy proxy)
        {
            mProxy = proxy;
        }

        public void Invoke(IWampRpcOperationCallback caller,
                           object options,
                           string procedure)
        {
            CallDetails callDetails = new CallDetails(caller, options, procedure);
            
            long requestId = RegisterCall(callDetails);

            mProxy.Call(requestId, options, procedure);
        }

        public void Invoke(IWampRpcOperationCallback caller,
                           object options,
                           string procedure,
                           object[] arguments)
        {
            CallDetails callDetails = new CallDetails(caller, options, procedure, arguments);

            long requestId = RegisterCall(callDetails);

            mProxy.Call(requestId, options, procedure, arguments);
        }

        public void Invoke(IWampRpcOperationCallback caller,
                           object options,
                           string procedure,
                           object[] arguments,
                           object argumentsKeywords)
        {
            CallDetails callDetails = new CallDetails(caller, options, procedure, arguments, argumentsKeywords);

            long requestId = RegisterCall(callDetails);

            mProxy.Call(requestId, options, procedure, arguments);
        }

        public void Result(long requestId, TMessage details)
        {
            CallDetails callDetails = TryGetCallDetails(requestId);

            if (callDetails != null)
            {
                callDetails.Caller.Result(details);
            }
        }

        public void Result(long requestId, TMessage details, TMessage[] arguments)
        {
            CallDetails callDetails = TryGetCallDetails(requestId);

            if (callDetails != null)
            {
                object[] castedArguments = arguments.Cast<object>().ToArray();
                callDetails.Caller.Result(details, castedArguments);
            }
        }

        public void Result(long requestId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            CallDetails callDetails = TryGetCallDetails(requestId);

            if (callDetails != null)
            {
                object[] castedArguments = arguments.Cast<object>().ToArray();
                callDetails.Caller.Result(details, castedArguments, argumentsKeywords);
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

        private class CallDetails
        {
            private readonly IWampRpcOperationCallback mCaller;
            private readonly object mOptions;
            private readonly string mProcedure;
            private readonly object[] mArguments;
            private readonly object mArgumentsKeywords;

            public CallDetails(IWampRpcOperationCallback caller, object options, string procedure, object[] arguments = null, object argumentsKeywords = null)
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

            public IWampRpcOperationCallback Caller
            {
                get
                {
                    return mCaller;
                }
            }

            public object Options
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

            public object ArgumentsKeywords
            {
                get
                {
                    return mArgumentsKeywords;
                }
            }
        }
    }
}