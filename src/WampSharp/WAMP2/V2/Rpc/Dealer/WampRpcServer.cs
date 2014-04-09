using System;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.Rpc
{
    public class WampRpcServer<TMessage> : IWampRpcServer<TMessage>,
        IWampErrorCallback<TMessage> 
    {
        private readonly IWampFormatter<TMessage> mFormatter; 
        private readonly IWampRpcOperationInvoker mInvoker;
        private readonly IWampCalleeOperationCatalog<TMessage> mCalleeCatalog;
        private readonly IWampCalleeInvocationHandler<TMessage> mHandler; 

        public WampRpcServer(IWampRpcOperationCatalog catalog, IWampBinding<TMessage> binding)
        {
            mInvoker = catalog;
            mFormatter = binding.Formatter;

            mHandler = new WampCalleeInvocationHandler<TMessage>();

            mCalleeCatalog = new WampCalleeOperationCatalog<TMessage>
                (catalog, mHandler);
        }

        public void Register(IWampCallee callee, long requestId, TMessage options, string procedure)
        {
            try
            {
                long registrationId = mCalleeCatalog.Register(callee, options, procedure);
                callee.Registered(requestId, registrationId);
            }
            catch (Exception exception)
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
            catch (Exception exception)
            {
                callee.UnregisterError(requestId, exception);
            }
        }

        public void Call(IWampCaller caller, long requestId, TMessage options, string procedure)
        {
            // TODO: clean requests of disconnected callers.
            IWampRpcOperationCallback callback = GetCallback(caller, requestId);

            mInvoker.Invoke(callback, mFormatter, options, procedure);
        }

        public void Call(IWampCaller caller, long requestId, TMessage options, string procedure, TMessage[] arguments)
        {
            IWampRpcOperationCallback callback = GetCallback(caller, requestId);

            mInvoker.Invoke(callback, mFormatter, options, procedure, arguments);
        }

        public void Call(IWampCaller caller, long requestId, TMessage options, string procedure, TMessage[] arguments,
                         TMessage argumentsKeywords)
        {
            IWampRpcOperationCallback callback = GetCallback(caller, requestId);

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

        private IWampRpcOperationCallback GetCallback(IWampCaller caller, long requestId)
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