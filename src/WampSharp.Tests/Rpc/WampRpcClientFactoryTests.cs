using NUnit.Framework;
using WampSharp.Rpc;

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

            var mockWampRpcClientHandlerBuilder = new MockWampRpcClientHandlerBuilder(clientHandler);
            IWampRpcClientFactory clientFactory = new WampRpcClientFactory(serializer, mockWampRpcClientHandlerBuilder);

            ICalculator proxy = clientFactory.GetClient<ICalculator>();
            int nine = proxy.Square(3);

            WampRpcCall<object> wampRpcCall = clientHandler.LastMessage;
            Assert.That(wampRpcCall.ProcUri, Is.EqualTo("Square"));
            CollectionAssert.AreEqual(wampRpcCall.Arguments, new object[] { 3 });
        }
    }
}