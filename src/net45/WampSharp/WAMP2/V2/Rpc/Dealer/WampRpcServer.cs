using System;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcServer<TMessage> : IWampRpcServer<TMessage>,
        IWampErrorCallback<TMessage> 
    {
        private readonly IWampFormatter<TMessage> mFormatter; 
        private readonly IWampRpcOperationInvoker mInvoker;
        private readonly IWampCalleeOperationCatalog mCalleeCatalog;
        private readonly IWampCalleeInvocationHandler<TMessage> mHandler; 

        public WampRpcServer(IWampRpcOperationCatalog catalog, IWampBinding<TMessage> binding)
        {
            mInvoker = catalog;
            mFormatter = binding.Formatter;

            mHandler = new WampCalleeInvocationHandler<TMessage>(binding.Formatter);

            mCalleeCatalog = new WampCalleeOperationCatalog<TMessage>
                (catalog, mHandler);
        }

        public void Register(IWampCallee callee, long requestId, RegisterOptions options, string procedure)
        {
            try
            {
                RegisterRequest registerRequest = new RegisterRequest(callee, requestId);
                mCalleeCatalog.Register(registerRequest, options, procedure);
            }
            catch (WampException exception)
            {
                callee.RegisterError(requestId, exception);
            }
        }

        public void Unregister(IWampCallee callee, long requestId, long registrationId)
        {
            try
            {
                mCalleeCatalog.Unregister(callee, registrationId);
                callee.Unregistered(requestId);
            }
            catch (WampException exception)
            {
                callee.UnregisterError(requestId, exception);
            }
        }

        public void Call(IWampCaller caller, long requestId, TMessage options, string procedure)
        {
            IWampRawRpcOperationCallback callback = GetCallback(caller, requestId);

            mInvoker.Invoke(callback, mFormatter, options, procedure);
        }

        public void Call(IWampCaller caller, long requestId, TMessage options, string procedure, TMessage[] arguments)
        {
            IWampRawRpcOperationCallback callback = GetCallback(caller, requestId);

            mInvoker.Invoke(callback, mFormatter, options, procedure, arguments);
        }

        public void Call(IWampCaller caller, long requestId, TMessage options, string procedure, TMessage[] arguments,
                         TMessage argumentsKeywords)
        {
            IWampRawRpcOperationCallback callback = GetCallback(caller, requestId);

            mInvoker.Invoke(callback, mFormatter, options, procedure, arguments, argumentsKeywords);
        }

        public void Cancel(IWampCaller caller, long requestId, TMessage options)
        {
            throw new NotImplementedException();
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options)
        {
            mHandler.Yield(callee, requestId, options);
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options, TMessage[] arguments)
        {
            mHandler.Yield(callee, requestId, options, arguments);
        }

        public void Yield(IWampCallee callee, long requestId, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mHandler.Yield(callee, requestId, options, arguments, argumentsKeywords);
        }

        private IWampRawRpcOperationCallback GetCallback(IWampCaller caller, long requestId)
        {
            return new WampRpcOperationCallback(caller, requestId);
        }

        public void Error(IWampClient client, int requestType, long requestId, TMessage details, string error)
        {
            mHandler.Error(client, requestId, details, error);
        }

        public void Error(IWampClient client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments)
        {
            mHandler.Error(client, requestId, details, error, arguments);
        }

        public void Error(IWampClient client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments,
                          TMessage argumentsKeywords)
        {
            mHandler.Error(client, requestId, details, error, arguments, argumentsKeywords);
        }
    }
}