using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using WampSharp.Core.Logs;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcServer<TMessage> : IWampRpcServer<TMessage>,
        IWampErrorCallback<TMessage>
    {
        private readonly ILogger mLogger;
        private readonly IWampFormatter<TMessage> mFormatter; 
        private readonly IWampRpcOperationInvoker mInvoker;
        private readonly IWampCalleeOperationCatalog mCalleeCatalog;
        private readonly IWampCalleeInvocationHandler<TMessage> mHandler; 

        public WampRpcServer(IWampRpcOperationCatalog catalog, IWampBinding<TMessage> binding)
        {
            mInvoker = catalog;
            mLogger = WampLoggerFactory.Create(this.GetType());
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
                mLogger.ErrorFormat(exception,
                    "Failed registering procedure '{0}'. Registration request id: {1} ",
                    procedure, requestId);

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
                mLogger.ErrorFormat(exception,
                    "Failed unregistering procedure with registration id {0}. Unregistration request id: {1} ",
                    registrationId,
                    requestId);

                callee.UnregisterError(requestId, exception);
            }
        }

        public void Call(IWampCaller caller, long requestId, CallOptions options, string procedure)
        {
            IWampRawRpcOperationClientCallback callback = GetCallback(caller, requestId);

            InvocationDetails invocationOptions =
                GetInvocationOptions(caller, options);

            mInvoker.Invoke(callback, mFormatter, invocationOptions, procedure);
        }

        public void Call(IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments)
        {
            IWampRawRpcOperationClientCallback callback = GetCallback(caller, requestId);

            InvocationDetails invocationOptions = 
                GetInvocationOptions(caller, options);

            mInvoker.Invoke(callback, mFormatter, invocationOptions, procedure, arguments);
        }

        public void Call(IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            IWampRawRpcOperationClientCallback callback = GetCallback(caller, requestId);

            InvocationDetails invocationOptions =
                GetInvocationOptions(caller, options);

            mInvoker.Invoke(callback, mFormatter, invocationOptions, procedure, arguments, 
                argumentsKeywords);
        }

        private InvocationDetails GetInvocationOptions(IWampCaller caller, CallOptions options)
        {
            InvocationDetailsExtended result = new InvocationDetailsExtended();

            IWampClientProxy wampCaller = caller as IWampClientProxy;
            result.CallerSession = wampCaller.Session;
            result.CallerOptions = options;

            return result;
        }

        public void Cancel(IWampCaller caller, long requestId, CancelOptions options)
        {
            throw new NotImplementedException();
        }

        public void Yield(IWampCallee callee, long requestId, YieldOptions options)
        {
            mHandler.Yield(callee, requestId, options);
        }

        public void Yield(IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments)
        {
            mHandler.Yield(callee, requestId, options, arguments);
        }

        public void Yield(IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            mHandler.Yield(callee, requestId, options, arguments, argumentsKeywords);
        }

        private IWampRawRpcOperationClientCallback GetCallback(IWampCaller caller, long requestId)
        {
            return new WampRpcOperationCallback(caller, requestId);
        }

        public void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error)
        {
            mHandler.Error(client, requestId, details, error);
        }

        public void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments)
        {
            mHandler.Error(client, requestId, details, error, arguments);
        }

        public void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments,
                          TMessage argumentsKeywords)
        {
            mHandler.Error(client, requestId, details, error, arguments, argumentsKeywords);
        }
    }
}