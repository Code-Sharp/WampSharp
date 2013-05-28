using System;
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
        public void HandleAsyncCallsServerProxyCall(WampRpcCall<object> rpcCall)
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

        private static WampRpcClientHandler<MockRaw> GetHandler(Func<IWampRpcClient<MockRaw>, IWampServer> serverFactory)
        {
            return new WampRpcClientHandler<MockRaw>(new MockWampRpcServerProxyFactory<MockRaw>(serverFactory),
                                                     new MockRawFormatter());
        }
    }
}