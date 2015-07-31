using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class WampClientRouterCallbackAdapter : IWampRawRpcOperationRouterCallback,
        ICallbackDisconnectionNotifier
    {
        private readonly IWampRawRpcOperationClientCallback mCaller;
        private readonly InvocationDetails mOptions;
        private readonly ICallbackDisconnectionNotifier mNotifier;

        public WampClientRouterCallbackAdapter(IWampRawRpcOperationClientCallback caller, InvocationDetails options)
        {
            mCaller = caller;
            mNotifier = mCaller as ICallbackDisconnectionNotifier;
            mNotifier.Disconnected += OnDisconnected;
            mOptions = options;
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            RaiseDisconnected();
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, YieldOptions details)
        {
            ResultDetails resultDetails = GetResultDetails(details);
            mCaller.Result(formatter, resultDetails);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, YieldOptions details, TMessage[] arguments)
        {
            ResultDetails resultDetails = GetResultDetails(details);
            mCaller.Result(formatter, resultDetails, arguments);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, YieldOptions details, TMessage[] arguments,
                                     IDictionary<string, TMessage> argumentsKeywords)
        {
            ResultDetails resultDetails = GetResultDetails(details);
            mCaller.Result(formatter, resultDetails, arguments, argumentsKeywords);
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
        {
            mCaller.Error(formatter, details, error);
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments)
        {
            mCaller.Error(formatter, details, error, arguments);
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments,
                                    TMessage argumentsKeywords)
        {
            mCaller.Error(formatter, details, error, arguments, argumentsKeywords);
        }

        private ResultDetails GetResultDetails(YieldOptions details)
        {
            return new ResultDetails {Progress = details.Progress};
        }

        public event EventHandler Disconnected;

        private void RaiseDisconnected()
        {
            EventHandler handler = Disconnected;
            
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}