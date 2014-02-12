using NUnit.Framework;
using WampSharp.Tests.Rpc.Helpers;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Rpc.Client;

namespace WampSharp.Tests.Rpc
{
    [TestFixture]
    public class WampRpcClientFactoryTests
    {
        [Test]
        public void RpcClientFactoryCallHandlerWithSerializedCall()
        {
            var delegateProcUriMapper = new WampDelegateProcUriMapper(methodInfo => methodInfo.Name);
            
            IWampRpcSerializer serializer = new WampRpcSerializer(delegateProcUriMapper); // I'm not sure if we want to mock this
            var clientHandler = new MockWampRpcClientHandler(4);

            var mockWampRpcClientHandlerBuilder = new MockWampRpcClientHandlerBuilder<MockRaw>(clientHandler);
            IWampRpcClientFactory<MockRaw> clientFactory = new WampRpcClientFactory<MockRaw>(serializer, mockWampRpcClientHandlerBuilder);

            ICalculator proxy = clientFactory.GetClient<ICalculator>(DummyConnection<MockRaw>.Instance);
            int nine = proxy.Square(3);

            WampRpcCall wampRpcCall = clientHandler.LastMessage;
            Assert.That(wampRpcCall.ProcUri, Is.EqualTo("Square"));
            CollectionAssert.AreEqual(wampRpcCall.Arguments, new object[] { 3 });
        }
    }
}