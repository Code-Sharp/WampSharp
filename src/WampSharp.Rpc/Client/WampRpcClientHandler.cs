using System;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using WampSharp.Core.Contracts.V1;
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

        public Task<object> HandleAsync(WampRpcCall<object> rpcCall)
        {
            rpcCall.CallId = Guid.NewGuid().ToString(); 
            // TODO: replace this with CallIdGenerator
            WampRpcRequest wampRpcRequest = new WampRpcRequest();
            ISubject<object> task = new ReplaySubject<object>(1);

            wampRpcRequest.Request = rpcCall;
            wampRpcRequest.Task = task;

            mCallIdToSubject[rpcCall.CallId] = wampRpcRequest;

            mServerProxy.Call(null, rpcCall.CallId, rpcCall.ProcUri, rpcCall.Arguments);

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
                // Probably this is some other client's call result.
            }
        }

        private class RpcWampClient : IWampClient<TMessage>
        {
            private readonly WampRpcClientHandler<TMessage> mParent;
            private string mSessionId;

            public RpcWampClient(WampRpcClientHandler<TMessage> parent)
            {
                mParent = parent;
            }

            public void Welcome(string sessionId, int protocolVersion, string serverIdent)
            {
                mSessionId = sessionId;
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

            public string SessionId
            {
                get
                {
                    return mSessionId;
                }
            }
        }

    }
}