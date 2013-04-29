using System.Collections.Concurrent;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using WampSharp.Core.Client;
using WampSharp.Core.Contracts;
using WampSharp.Core.Serialization;

namespace WampSharp.Rpc
{
    public class WampRpcClientHandler<TMessage> : IWampRpcClientHandler
    {
        private readonly IWampServer<object> mServerProxy;

        private readonly ConcurrentDictionary<string, WampRpcRequest> mCallIdToSubject = 
            new ConcurrentDictionary<string, WampRpcRequest>();

        private readonly IWampFormatter<TMessage> mFormatter;

        public WampRpcClientHandler(IWampServerProxyFactory<TMessage> serverProxyFactory, IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
            mServerProxy = serverProxyFactory.Create(new RpcWampClient(this));
        }

        public object Handle(WampRpcCall<object> rpcCall)
        {
            Task<object> task = HandleAsync(rpcCall);
            return task.Result;
        }

        private Task<object> HandleAsync(WampRpcCall<object> rpcCall)
        {
            mServerProxy.Call(null, rpcCall.CallId, rpcCall.ProcUri, rpcCall.Arguments);

            WampRpcRequest wampRpcRequest = new WampRpcRequest();
            Subject<object> task = new Subject<object>();

            wampRpcRequest.Request = rpcCall;
            wampRpcRequest.Task = task;

            mCallIdToSubject[rpcCall.CallId] = wampRpcRequest;

            return task.ToTask();
        }

        private void ResultArrived(string callId, TMessage result)
        {
            WampRpcRequest request;
            
            if (mCallIdToSubject.TryRemove(callId, out request))
            {
                object deserialized = mFormatter.Deserialize(request.Request.ReturnType, result);
                ISubject<object> task = request.Task;
                task.OnNext(deserialized);
                task.OnCompleted();
            }
            else
            {
                // Log?
            }
        }

        private class RpcWampClient : IWampClient<TMessage>
        {
            private readonly WampRpcClientHandler<TMessage> mParent;

            public RpcWampClient(WampRpcClientHandler<TMessage> parent)
            {
                mParent = parent;
            }

            public void Welcome(string sessionId, int protocolVersion, string serverIdent)
            {
                throw new System.NotImplementedException();
            }

            public void CallResult(string callId, TMessage result)
            {
                mParent.ResultArrived(callId, result);
            }

            public void CallError(string callId, string errorUri, string errorDesc)
            {
            }

            public void CallError(string callId, string errorUri, string errorDesc, TMessage errorDetails)
            {
                throw new System.NotImplementedException();
            }

            public void Event(string topicUri, TMessage @event)
            {
                throw new System.NotImplementedException();
            }

            public string SessionId { get; private set; }
        }

    }
}