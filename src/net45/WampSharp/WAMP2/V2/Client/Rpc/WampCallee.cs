using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    internal class WampCallee<TMessage> : IWampRpcOperationRegistrationProxy, IWampCallee<TMessage>,
        IWampCalleeError<TMessage>
    {
        private readonly IWampServerProxy mProxy;

        private readonly IWampFormatter<TMessage> mFormatter;

        private readonly WampIdMapper<Request> mPendingRegistrations =
            new WampIdMapper<Request>();

        private readonly ConcurrentDictionary<long, IWampRpcOperation> mRegistrations =
            new ConcurrentDictionary<long, IWampRpcOperation>();

        private readonly ConcurrentDictionary<IWampRpcOperation, long> mOperationToRegistrationId =
            new ConcurrentDictionary<IWampRpcOperation, long>();

        private readonly WampIdMapper<Request> mPendingUnregistrations =
            new WampIdMapper<Request>();

        public WampCallee(IWampServerProxy proxy, IWampFormatter<TMessage> formatter)
        {
            mProxy = proxy;
            mFormatter = formatter;
        }

        public Task Register(IWampRpcOperation operation, object options)
        {
            Request request =
                new Request(operation, mFormatter);

            long id = mPendingRegistrations.Add(request);

            request.RequestId = id;

            mProxy.Register(id, options, operation.Procedure);

            return request.Task;
        }

        public void Registered(long requestId, long registrationId)
        {
            Request request;

            if (mPendingRegistrations.TryRemove(requestId, out request))
            {
                mRegistrations[registrationId] = request.Operation;
                mOperationToRegistrationId[request.Operation] = registrationId;
                request.Complete();
            }
        }

        public Task Unregister(IWampRpcOperation operation)
        {
            Request request =
                new Request(operation, mFormatter);

            long registrationId;

            if (!TryGetOperationRegistrationId(operation, out registrationId))
            {
                return null;
            }
            else
            {
                long requestId = mPendingUnregistrations.Add(request);

                request.RequestId = requestId;

                mProxy.Unregister(requestId, registrationId);

                return request.Task;
            }
        }

        private bool TryGetOperationRegistrationId(IWampRpcOperation operation, out long registrationId)
        {
            return mOperationToRegistrationId.TryGetValue(operation, out registrationId);
        }

        public void Unregistered(long requestId)
        {
            Request unregistration;

            if (mPendingUnregistrations.TryRemove(requestId, out unregistration))
            {
                IWampRpcOperation operation;
                long registrationId;
                mRegistrations.TryRemove(requestId, out operation);
                mOperationToRegistrationId.TryRemove(unregistration.Operation, out registrationId);
                unregistration.Complete();
            }
        }

        private IWampRpcOperation TryGetOperation(long registrationId)
        {
            IWampRpcOperation operation;

            if (mRegistrations.TryGetValue(registrationId, out operation))
            {
                return operation;
            }

            return null;
        }

        private IWampRpcOperationCallback GetCallback(long requestId)
        {
            return new ServerProxyCallback(mProxy, requestId);
        }

        public void Invocation(long requestId, long registrationId, TMessage details)
        {
            IWampRpcOperation operation = TryGetOperation(registrationId);

            if (operation != null)
            {
                IWampRpcOperationCallback callback = GetCallback(requestId);
                operation.Invoke(callback, mFormatter, details);
            }
        }

        public void Invocation(long requestId, long registrationId, TMessage details, TMessage[] arguments)
        {
            IWampRpcOperation operation = TryGetOperation(registrationId);

            if (operation != null)
            {
                IWampRpcOperationCallback callback = GetCallback(requestId);
                operation.Invoke(callback, mFormatter, details, arguments);
            }
        }

        public void Invocation(long requestId, long registrationId, TMessage details, TMessage[] arguments,
                               TMessage argumentsKeywords)
        {
            IWampRpcOperation operation = TryGetOperation(registrationId);

            if (operation != null)
            {
                IWampRpcOperationCallback callback = GetCallback(requestId);
                operation.Invoke(callback, mFormatter, details, arguments, argumentsKeywords);
            }
        }

        public void RegisterError(long requestId, TMessage details, string error)
        {
            Request request;

            if (mPendingRegistrations.TryRemove(requestId, out request))
            {
                request.Error(details, error);
            }
        }

        public void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            Request request;

            if (mPendingRegistrations.TryRemove(requestId, out request))
            {
                request.Error(details, error, arguments);
            }
        }

        public void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            Request request;

            if (mPendingRegistrations.TryRemove(requestId, out request))
            {
                request.Error(details, error, arguments, argumentsKeywords);
            }
        }

        public void UnregisterError(long requestId, TMessage details, string error)
        {
            Request request;

            if (mPendingUnregistrations.TryRemove(requestId, out request))
            {
                request.Error(details, error);
            }
        }

        public void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            Request request;

            if (mPendingUnregistrations.TryRemove(requestId, out request))
            {
                request.Error(details, error, arguments);
            }
        }

        public void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            Request request;

            if (mPendingUnregistrations.TryRemove(requestId, out request))
            {
                request.Error(details, error, arguments, argumentsKeywords);
            }
        }

        public void Interrupt(long requestId, TMessage options)
        {
            throw new System.NotImplementedException();
        }

        private class Request : WampPendingRequest<TMessage>
        {
            private readonly IWampRpcOperation mOperation;

            public Request(IWampRpcOperation operation, IWampFormatter<TMessage> formatter) :
                base(formatter)
            {
                mOperation = operation;
            }

            public IWampRpcOperation Operation
            {
                get
                {
                    return mOperation;
                }
            }
        }

        private class ServerProxyCallback : IWampRpcOperationCallback
        {
            private readonly IWampServerProxy mProxy;
            private readonly long mRequestId;

            public ServerProxyCallback(IWampServerProxy proxy, long requestId)
            {
                mProxy = proxy;
                mRequestId = requestId;
            }

            public long RequestId
            {
                get
                {
                    return mRequestId;
                }
            }

            public void Result(object details)
            {
                mProxy.Yield(RequestId, details);
            }

            public void Result(object details, object[] arguments)
            {
                mProxy.Yield(RequestId, details, arguments);
            }

            public void Result(object details, object[] arguments, object argumentsKeywords)
            {
                mProxy.Yield(RequestId, details, arguments, argumentsKeywords);
            }

            public void Error(object details, string error)
            {
                mProxy.InvocationError(RequestId, details, error);
            }

            public void Error(object details, string error, object[] arguments)
            {
                mProxy.InvocationError(RequestId, details, error, arguments);
            }

            public void Error(object details, string error, object[] arguments, object argumentsKeywords)
            {
                mProxy.InvocationError(RequestId, details, error, arguments, argumentsKeywords);
            }
        }
    }
}