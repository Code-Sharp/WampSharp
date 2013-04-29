using System.Reflection;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Client;
using WampSharp.Core.Contracts;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Rpc;
using WampSharp.Core.Serialization;
using WampSharp.Fleck;

namespace WampSharp.Tests
{
    [TestFixture]
    public class RpcTests
    {
        private IWampFormatter<JToken> mFormatter;

        [SetUp]
        public void SetUpFormatter()
        {
            mFormatter = new JsonFormatter();
        }

        [Test]
        public void ClientShouldBeAbleToCallMethodOnTheServer()
        {
            var rpcFactory = new RpcFactory();
            IWampServer server = null;
            ICalculator proxy = rpcFactory.GetProxy<ICalculator>(server);
            int four = proxy.Square(2);

            Assert.That(four, Is.EqualTo(4));
        }

        [Test]
        public void RpcClientFactoryCallHandlerWithSerializedCall()
        {
            var delegateProcUriMapper = new DelegateProcUriMapper(methodInfo => methodInfo.Name);
            IWampRpcSerializer serializer = new WampRpcSerializer(delegateProcUriMapper);
            var clientHandler = new MockWampRpcClientHandler(4);

            var mockWampRpcClientHandlerBuilder = new MockWampRpcClientHandlerBuilder(clientHandler);
            IWampRpcClientFactory clientFactory = new WampRpcClientFactory(serializer, mockWampRpcClientHandlerBuilder);

            ICalculator proxy = clientFactory.GetClient<ICalculator>();
            int nine = proxy.Square(3);

            WampRpcCall<object> wampRpcCall = clientHandler.LastMessage;
            Assert.That(wampRpcCall.ProcUri, Is.EqualTo("Square"));
            CollectionAssert.AreEqual(wampRpcCall.Arguments, new object[] { 3 });
        }


        public void AA()
        {
            
        }



    }



    public interface ICalculator
    {
        int Square(int number);
    }

    public class RpcFactory
    {
        // THIS IS THE IDEALIC IMPLEMENTATION!!!
        // PLEASE SUBMIT PULL REQUEST TO MAKE THIS HAPPEN! 
        public T GetProxy<T>(IWampServer server)
        {
            throw new System.NotImplementedException();
        }
    }

    class MockWampRpcClientHandler : IWampRpcClientHandler
    {
        private readonly object mResult;

        public MockWampRpcClientHandler(object result)
        {
            mResult = result;
        }

        public WampRpcCall<object> LastMessage { get; set; }
        public object Handle(WampRpcCall<object> rpcCall)
        {
            LastMessage = rpcCall;

            return mResult;
        }
    }

    class MockWampRpcClientHandlerBuilder : IWampRpcClientHandlerBuilder
    {
        private readonly MockWampRpcClientHandler mHandler;

        public MockWampRpcClientHandlerBuilder(MockWampRpcClientHandler handler)
        {
            mHandler = handler;
        }

        public IWampRpcClientHandler<object> Build()
        {
            return mHandler;
        }
    }
}