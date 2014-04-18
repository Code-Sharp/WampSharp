using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    internal class WampRpcOperationCatalogProxy<TMessage> :
        IWampRpcOperationCatalogProxy, IWampCallee<TMessage>, IWampCaller<TMessage>
    {
        private readonly WampCallee<TMessage> mCallee;
        private readonly WampCaller<TMessage> mCaller;

        public WampRpcOperationCatalogProxy(IWampServerProxy proxy, IWampFormatter<TMessage> formatter)
        {
            mCallee = new WampCallee<TMessage>(proxy, formatter);
            mCaller = new WampCaller<TMessage>(proxy, formatter);
        }

        public Task Register(IWampRpcOperation operation, object options)
        {
            return mCallee.Register(operation, options);
        }

        public void Registered(long requestId, long registrationId)
        {
            mCallee.Registered(requestId, registrationId);
        }

        public Task Unregister(IWampRpcOperation operation)
        {
            return mCallee.Unregister(operation);
        }

        public void Unregistered(long requestId)
        {
            mCallee.Unregistered(requestId);
        }

        public void Invocation(long requestId, long registrationId, TMessage details)
        {
            mCallee.Invocation(requestId, registrationId, details);
        }

        public void Invocation(long requestId, long registrationId, TMessage details, TMessage[] arguments)
        {
            mCallee.Invocation(requestId, registrationId, details, arguments);
        }

        public void Invocation(long requestId, long registrationId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mCallee.Invocation(requestId, registrationId, details, arguments, argumentsKeywords);
        }

        public void Invoke(IWampRawRpcOperationCallback caller, object options, string procedure)
        {
            mCaller.Invoke(caller, options, procedure);
        }

        public void Invoke(IWampRawRpcOperationCallback caller, object options, string procedure, object[] arguments)
        {
            mCaller.Invoke(caller, options, procedure, arguments);
        }

        public void Invoke(IWampRawRpcOperationCallback caller, object options, string procedure, object[] arguments,
                           object argumentsKeywords)
        {
            mCaller.Invoke(caller, options, procedure, arguments, argumentsKeywords);
        }

        public void Interrupt(long requestId, TMessage options)
        {
            mCallee.Interrupt(requestId, options);
        }

        public void Invoke(IWampRpcOperationCallback caller, object options, string procedure)
        {
            mCaller.Invoke(caller, options, procedure);
        }

        public void Invoke(IWampRpcOperationCallback caller, object options, string procedure, object[] arguments)
        {
            mCaller.Invoke(caller, options, procedure, arguments);
        }

        public void Invoke(IWampRpcOperationCallback caller, object options, string procedure, object[] arguments,
                           object argumentsKeywords)
        {
            mCaller.Invoke(caller, options, procedure, arguments, argumentsKeywords);
        }

        public void Result(long requestId, TMessage details)
        {
            mCaller.Result(requestId, details);
        }

        public void Result(long requestId, TMessage details, TMessage[] arguments)
        {
            mCaller.Result(requestId, details, arguments);
        }

        public void Result(long requestId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mCaller.Result(requestId, details, arguments, argumentsKeywords);
        }
    }
}