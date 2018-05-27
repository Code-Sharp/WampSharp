using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampRpcClientHandler"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampRpcClientHandler<TMessage> : IWampRpcClientHandler
    {
        private readonly IWampServer<object> mServerProxy;

        private readonly ConcurrentDictionary<string, WampRpcRequest> mCallIdToSubject = 
            new ConcurrentDictionary<string, WampRpcRequest>();

        private readonly IWampFormatter<TMessage> mFormatter;

        /// <summary>
        /// Creates a new instance of <see cref="WampRpcClientHandler{TMessage}"/>.
        /// </summary>
        public WampRpcClientHandler(IWampServerProxyFactory<TMessage> serverProxyFactory, IWampConnection<TMessage> connection, IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
            mServerProxy = serverProxyFactory.Create(new RpcWampClient(this), connection);

            connection.ConnectionClosed += Connection_ConnectionClosed;
            connection.ConnectionError += Connection_ConnectionError;
        }

        private void Connection_ConnectionError(object sender, WampConnectionErrorEventArgs e)
        {
            HandleConnectionIssue("Connection Error");
        }

        private void Connection_ConnectionClosed(object sender, EventArgs e)
        {
            HandleConnectionIssue("Connection Closed");
        }

        private void HandleConnectionIssue(string errorDesc)
        {
            foreach (var kvp in mCallIdToSubject)
            {
                ErrorArrived(kvp.Key, "http://api.wamp.ws/error#generic", errorDesc);
            }
        }

        public object Handle(WampRpcCall rpcCall)
        {
            Task<object> task = HandleAsync(rpcCall);

            try
            {
                return task.Result;
            }
            catch (AggregateException ex)
            {                
                throw ex.InnerException;
            }
        }

        public Task<object> HandleAsync(WampRpcCall rpcCall)
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

            if (mCallIdToSubject.TryRemove(callId, out WampRpcRequest request))
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

        private void ErrorArrived(string callId, string errorUri, string errorDesc, TMessage errorDetails = default(TMessage))
        {

            if (mCallIdToSubject.TryRemove(callId, out WampRpcRequest request))
            {
                object deserialized = null;
                if (errorDetails != null)
                {
                    deserialized = mFormatter.Deserialize(typeof(ExpandoObject), errorDetails);
                }

                ISubject<object> task = request.Task;
                task.OnError(new WampRpcCallException(request.Request.ProcUri,
                                                      callId,
                                                      errorUri,
                                                      errorDesc,
                                                      deserialized));
            }
            else
            {
                // Probably this is some other client's call result.
            }
        }

        private class RpcWampClient : IWampRpcClient<TMessage>
        {
            private readonly WampRpcClientHandler<TMessage> mParent;

            public RpcWampClient(WampRpcClientHandler<TMessage> parent)
            {
                mParent = parent;
            }

            public void CallResult(string callId, TMessage result)
            {
                mParent.ResultArrived(callId, result);
            }

            public void CallError(string callId, string errorUri, string errorDesc)
            {
                mParent.ErrorArrived(callId, errorUri, errorDesc);
            }

            public void CallError(string callId, string errorUri, string errorDesc, TMessage errorDetails)
            {
                mParent.ErrorArrived(callId, errorUri, errorDesc, errorDetails);
            }
        }
    }
}