using System;
using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Core.Contracts.V1;
using WampSharp.Rpc;
using WampSharp.Tests.Rpc.Helpers;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Rpc
{
    public class WampRpcClientHandlerTests
    {
        [TestCaseSource(typeof(RpcCalls), "SomeCalls")]
        public void HandleAsync_Calls_ServerProxyCall(WampRpcCall<object> rpcCall)
        {
            MockWampRpcCallManager<MockRaw> callManager =
                new MockWampRpcCallManager<MockRaw>();

            IWampRpcClientHandler handler =
                GetHandler(client => callManager.GetServer(client));

            Assert.That(callManager.AllCalls, Is.Empty);

            Task<object> task = handler.HandleAsync(rpcCall);

            MockWampRpcCallDetails<MockRaw> details =
                callManager.GetCallDetails(rpcCall.CallId);

            Assert.IsNotNull(details);

            Assert.That(details.CallId, Is.EqualTo(rpcCall.CallId));
            Assert.That(details.ProcUri, Is.EqualTo(rpcCall.ProcUri));
            CollectionAssert.AreEqual(rpcCall.Arguments, details.Arguments);
        }

        [TestCaseSource(typeof(RpcCalls), "CallsWithResults")]
        public void HandleAsync_ClientCallResult_SetsTasksResult(WampRpcCall<object> rpcCall, object result)
        {
            MockWampRpcCallManager<MockRaw> callManager =
                new MockWampRpcCallManager<MockRaw>();

            IWampRpcClientHandler handler =
                GetHandler(client => callManager.GetServer(client));

            if (result != null)
            {
                rpcCall.ReturnType = result.GetType();
            }

            Task<object> task = handler.HandleAsync(rpcCall);

            MockWampRpcCallDetails<MockRaw> details =
                callManager.GetCallDetails(rpcCall.CallId);

            Assert.IsFalse(task.IsCompleted);

            details.Client.CallResult(rpcCall.CallId, new MockRaw(result));

            Assert.That(task.Result, Is.EqualTo(result));
        }

        [TestCaseSource(typeof(RpcCalls), "CallsWithErrors")]
        public void HandleAsync_ClientCallError_SetsTasksException
            (WampRpcCall<object> rpcCall, CallErrorDetails callErrorDetails)
        {
            MockWampRpcCallManager<MockRaw> callManager =
                new MockWampRpcCallManager<MockRaw>();

            IWampRpcClientHandler handler =
                GetHandler(client => callManager.GetServer(client));

            Task<object> task = handler.HandleAsync(rpcCall);

            MockWampRpcCallDetails<MockRaw> details =
                callManager.GetCallDetails(rpcCall.CallId);

            Assert.IsFalse(task.IsCompleted);

            object errorDetails = callErrorDetails.ErrorDetails;
            
            if (errorDetails == null)
            {
                details.Client.CallError(rpcCall.CallId,
                                         callErrorDetails.ErrorUri,
                                         callErrorDetails.ErrorDesc);
            }
            else
            {
                details.Client.CallError(rpcCall.CallId,
                                         callErrorDetails.ErrorUri,
                                         callErrorDetails.ErrorDesc,
                                         new MockRaw(errorDetails));
            }

            AggregateException aggregatedException = task.Exception;
            Assert.IsNotNull(aggregatedException);

            Exception innerException = aggregatedException.InnerException;

            Assert.That(innerException, Is.TypeOf(typeof(WampRpcCallException)));

            WampRpcCallException casted = innerException as WampRpcCallException;
            Assert.That(casted.Message, Is.EqualTo(callErrorDetails.ErrorDesc));
            Assert.That(casted.CallId, Is.EqualTo(rpcCall.CallId));
            Assert.That(casted.ErrorUri, Is.EqualTo(callErrorDetails.ErrorUri));
            Assert.That(casted.ErrorDetails,
                        Is.EqualTo(errorDetails)
                          .Using(StructuralComparisons.StructuralEqualityComparer));
        }

        private static WampRpcClientHandler<MockRaw> GetHandler(Func<IWampRpcClient<MockRaw>, IWampServer> serverFactory)
        {
            return new WampRpcClientHandler<MockRaw>(new MockWampRpcServerProxyFactory<MockRaw>(serverFactory),
                                                     new MockRawFormatter());
        }
    }
}