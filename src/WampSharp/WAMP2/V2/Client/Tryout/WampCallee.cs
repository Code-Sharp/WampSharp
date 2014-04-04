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
    internal class WampCallee<TMessage> : IWampRpcOperationRegistrationProxy, IWampCallee<TMessage>
    {
        private readonly IWampServerProxy mProxy;

        private readonly IWampFormatter<TMessage> mFormatter;

        private readonly WampIdMapper<Request> mPendingRegistrations =
            new WampIdMapper<Request>();

        private readonly ConcurrentDictionary<long, IWampRpcOperation> mRegistrations =
            new ConcurrentDictionary<long, IWampRpcOperation>();

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
                new Request(operation);

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
                request.Complete();
            }
        }

        public Task Unregister(IWampRpcOperation operation)
        {
            Request request =
                new Request(operation);

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
            KeyValuePair<long, IWampRpcOperation> pair = 
                mRegistrations.FirstOrDefault(x => x.Value == operation);

            if (pair.Value == operation)
            {
                registrationId = pair.Key;
                return true;
            }

            registrationId = default(long);
            return false;
        }

        public void Unregistered(long requestId)
        {
            Request unregistration;

            if (mPendingUnregistrations.TryRemove(requestId, out unregistration))
            {
                IWampRpcOperation operation;
                mRegistrations.TryRemove(requestId, out operation);
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

        public void Interrupt(long requestId, TMessage options)
        {
            throw new System.NotImplementedException();
        }

        private class Request
        {
            private readonly TaskCompletionSource<bool> mTaskCompletionSource =
                new TaskCompletionSource<bool>();

            private readonly IWampRpcOperation mOperation;

            public Request(IWampRpcOperation operation)
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

            public long RequestId
            {
                get; 
                set;
            }

            public Task Task
            {
                get { return mTaskCompletionSource.Task; }
            }

            public void Complete()
            {
                mTaskCompletionSource.SetResult(true);
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
                mProxy.CallError(RequestId, details, error);
            }

            public void Error(object details, string error, object[] arguments)
            {
                mProxy.CallError(RequestId, details, error, arguments);
            }

            public void Error(object details, string error, object[] arguments, object argumentsKeywords)
            {
                mProxy.CallError(RequestId, details, error, arguments, argumentsKeywords);
            }
        }
    }
}