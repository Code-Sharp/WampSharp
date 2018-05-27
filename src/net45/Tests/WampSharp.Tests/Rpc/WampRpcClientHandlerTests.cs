using System;
using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Tests.Rpc.Helpers;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Rpc;
using WampSharp.V1.Rpc.Client;

namespace WampSharp.Tests.Rpc
{
    [TestFixture]
    public class WampRpcClientHandlerTests
    {
        [TestCaseSource(typeof(RpcCalls), nameof(RpcCalls.SomeCalls))]
        public void HandleAsync_Calls_ServerProxyCall(WampRpcCall rpcCall)
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

        [TestCaseSource(typeof(RpcCalls), nameof(RpcCalls.CallsWithResults))]
        public void HandleAsync_ClientCallResult_SetsTasksResult(WampRpcCall rpcCall, object result)
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

        [TestCaseSource(typeof(RpcCalls), nameof(RpcCalls.CallsWithErrors))]
        public void HandleAsync_ClientCallError_SetsTasksException
            (WampRpcCall rpcCall, CallErrorDetails callErrorDetails)
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

        [Test]
        public void HandleAsync_ClientCall_TaskIsAsync()
        {
            MockWampRpcCallManager<MockRaw> callManager =
                new MockWampRpcCallManager<MockRaw>();

            IWampRpcClientHandler handler =
                GetHandler(client => callManager.GetServer(client));

            // call a function that takes a long time, call another function
            // the result of the latter is received first, in other words,
            // RPC is really asynchronous
            var slowCall = new WampRpcCall()
                               {
                                   Arguments = new object[] {new int[] {1, 2, 3}},
                                   ProcUri = "calc:asum",
                                   ReturnType = typeof (int)
                               };

            var fastCall = new WampRpcCall()
                               {
                                   Arguments = new object[] {new int[] {4, 5, 6}},
                                   ProcUri = "calc:sum",
                                   ReturnType = typeof (int)
                               };

            Task<object> slowTask = handler.HandleAsync(slowCall);

            Task<object> fastTask = handler.HandleAsync(fastCall);

            MockWampRpcCallDetails<MockRaw> slowCallDetails =
                callManager.GetCallDetails(slowCall.CallId);

            MockWampRpcCallDetails<MockRaw> fastCallDetails =
                callManager.GetCallDetails(fastCall.CallId);
            
            Assert.IsFalse(slowTask.IsCompleted);
            Assert.IsFalse(fastTask.IsCompleted);

            fastCallDetails.Client.CallResult(fastCall.CallId, new MockRaw(15));

            Assert.That(fastTask.Result, Is.EqualTo(15));
            Assert.IsFalse(slowTask.IsCompleted);

            slowCallDetails.Client.CallResult(slowCall.CallId, new MockRaw(6));
            Assert.That(slowTask.Result, Is.EqualTo(6));            
        }

        private static WampRpcClientHandler<MockRaw> GetHandler(Func<IWampRpcClient<MockRaw>, IWampServer> serverFactory)
        {
            return new WampRpcClientHandler<MockRaw>(new MockWampRpcServerProxyFactory<MockRaw>(serverFactory),
                                                     DummyConnection<MockRaw>.Instance,
                                                     new MockRawFormatter());
        }
    }
}