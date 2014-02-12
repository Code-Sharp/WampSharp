using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcServer<TMessage> : IWampRpcServer<TMessage>,
        IWampErrorCallback<TMessage> 
        where TMessage : class
    {
        private readonly IWampRpcOperationInvoker<TMessage> mInvoker;
        private readonly IWampCalleeOperationCatalog<TMessage> mCalleeCatalog;
        private readonly IWampCalleeInvocationHandler<TMessage> mHandler; 

        public WampRpcServer(IWampRpcOperationCatalog<TMessage> catalog)
        {
            mInvoker = catalog;
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
                // TODO: Get details
                Exception details = exception;
                // TODO: Get error:
                string error = details.Message; 

                callee.RegisterError(requestId, details, error);
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
                // TODO: Get details
                Exception details = exception;
                // TODO: Get error:
                string error = details.Message;

                callee.UnregisterError(requestId, details, error);
            }
        }

        public void Call(IWampCaller caller, long requestId, TMessage options, string procedure)
        {
            IWampRpcOperationCallback<TMessage> callback = GetCallback(caller, requestId);

            mInvoker.Invoke(callback, options, procedure);
        }

        public void Call(IWampCaller caller, long requestId, TMessage options, string procedure, TMessage[] arguments)
        {
            IWampRpcOperationCallback<TMessage> callback = GetCallback(caller, requestId);

            mInvoker.Invoke(callback, options, procedure, arguments);
        }

        public void Call(IWampCaller caller, long requestId, TMessage options, string procedure, TMessage[] arguments,
                         TMessage argumentsKeywords)
        {
            IWampRpcOperationCallback<TMessage> callback = GetCallback(caller, requestId);

            mInvoker.Invoke(callback, options, procedure, arguments, argumentsKeywords);
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

        private IWampRpcOperationCallback<TMessage> GetCallback(IWampCaller caller, long requestId)
        {
            return new WampRpcOperationCallback<TMessage>(caller, requestId);
        }

        public void Error(IWampClient client, int reqestType, long requestId, TMessage details, string error)
        {
            mHandler.Error(client, requestId, details, error);
        }

        public void Error(IWampClient client, int reqestType, long requestId, TMessage details, string error, TMessage[] arguments)
        {
            mHandler.Error(client, requestId, details, error, arguments);
        }

        public void Error(IWampClient client, int reqestType, long requestId, TMessage details, string error, TMessage[] arguments,
                          TMessage argumentsKeywords)
        {
            mHandler.Error(client, requestId, details, error, arguments, argumentsKeywords);
        }
    }
}