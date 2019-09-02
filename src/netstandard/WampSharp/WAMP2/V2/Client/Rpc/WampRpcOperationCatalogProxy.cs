using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    internal class WampRpcOperationCatalogProxy<TMessage> :
        IWampRpcOperationCatalogProxy, IWampCallee<TMessage>, IWampCaller<TMessage>,
        IWampCalleeError<TMessage>, IWampCallerError<TMessage>
    {
        private readonly WampCallee<TMessage> mCallee;
        private readonly WampCaller<TMessage> mCaller;

        public WampRpcOperationCatalogProxy(IWampServerProxy proxy, IWampFormatter<TMessage> formatter, IWampClientConnectionMonitor monitor)
        {
            mCallee = new WampCallee<TMessage>(proxy, formatter, monitor);
            mCaller = new WampCaller<TMessage>(proxy, formatter, monitor);
        }

        public Task<IAsyncDisposable> Register(IWampRpcOperation operation, RegisterOptions options)
        {
            return mCallee.Register(operation, options);
        }

        public void Registered(long requestId, long registrationId)
        {
            mCallee.Registered(requestId, registrationId);
        }

        public void Unregistered(long requestId)
        {
            mCallee.Unregistered(requestId);
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details)
        {
            mCallee.Invocation(requestId, registrationId, details);
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details, TMessage[] arguments)
        {
            mCallee.Invocation(requestId, registrationId, details, arguments);
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            mCallee.Invocation(requestId, registrationId, details, arguments, argumentsKeywords);
        }

        public IWampCancellableInvocationProxy Invoke(IWampRawRpcOperationClientCallback caller, CallOptions options, string procedure)
        {
            return mCaller.Invoke(caller, options, procedure);
        }

        public IWampCancellableInvocationProxy Invoke(IWampRawRpcOperationClientCallback caller, CallOptions options, string procedure, object[] arguments)
        {
            return mCaller.Invoke(caller, options, procedure, arguments);
        }

        public IWampCancellableInvocationProxy Invoke(IWampRawRpcOperationClientCallback caller, CallOptions options, string procedure, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            return mCaller.Invoke(caller, options, procedure, arguments, argumentsKeywords);
        }

        public void Interrupt(long requestId, InterruptDetails details)
        {
            mCallee.Interrupt(requestId, details);
        }

        public void Result(long requestId, ResultDetails details)
        {
            mCaller.Result(requestId, details);
        }

        public void Result(long requestId, ResultDetails details, TMessage[] arguments)
        {
            mCaller.Result(requestId, details, arguments);
        }

        public void Result(long requestId, ResultDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            mCaller.Result(requestId, details, arguments, argumentsKeywords);
        }

        public void RegisterError(long requestId, TMessage details, string error)
        {
            mCallee.RegisterError(requestId, details, error);
        }

        public void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            mCallee.RegisterError(requestId, details, error, arguments);
        }

        public void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mCallee.RegisterError(requestId, details, error, arguments, argumentsKeywords);
        }

        public void UnregisterError(long requestId, TMessage details, string error)
        {
            mCallee.UnregisterError(requestId, details, error);
        }

        public void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            mCallee.UnregisterError(requestId, details, error, arguments);
        }

        public void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mCallee.UnregisterError(requestId, details, error, arguments, argumentsKeywords);
        }

        public void CallError(long requestId, TMessage details, string error)
        {
            mCaller.CallError(requestId, details, error);
        }

        public void CallError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            mCaller.CallError(requestId, details, error, arguments);
        }

        public void CallError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mCaller.CallError(requestId, details, error, arguments, argumentsKeywords);
        }
    }
}