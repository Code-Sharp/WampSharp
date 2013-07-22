﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Client;
using WampSharp.Core.Contracts;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Newtonsoft;
using WampSharp.Rpc;
using WampSharp.Core.Serialization;
using WampSharp.Fleck;
using WampSharp.Rpc.Client;
using WampSharp.Tests.TestHelpers;

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
        public void RpcClientFactoryCallCallOnProxyAndWait()
        {
            var delegateProcUriMapper = new WampDelegateProcUriMapper(methodInfo => methodInfo.Name);
            IWampRpcSerializer serializer = new WampRpcSerializer(delegateProcUriMapper);

            MockWampServerProxyFactory<JToken> mockWampServerProxyFactory = null;

            Mock<IWampServer> serverMock = new Mock<IWampServer>();
            serverMock.Setup(x => x.Call(It.IsAny<IWampClient>(),
                                         It.IsAny<string>(),
                                         It.IsAny<string>(),
                                         It.IsAny<object[]>()))
                      .Callback((IWampClient client, string callId, string procUri, object[] args) =>
                                mockWampServerProxyFactory.Client.CallResult(callId, JToken.FromObject(3)));

            mockWampServerProxyFactory = new MockWampServerProxyFactory<JToken>(serverMock.Object);
            
            WampRpcClientHandlerBuilder<JToken> mockWampRpcClientHandlerBuilder = 
                new WampRpcClientHandlerBuilder<JToken>
                (new JsonFormatter(),
                 mockWampServerProxyFactory);

            IWampRpcClientFactory<JToken> clientFactory = new WampRpcClientFactory<JToken>(serializer, mockWampRpcClientHandlerBuilder);

            ICalculator proxy = clientFactory.GetClient<ICalculator>(DummyConnection<JToken>.Instance);
            int three = proxy.Square(3);

            Assert.That(three, Is.EqualTo(3));
        }


        public void AA()
        {
            
        }



    }

    public class MockFormatter : IWampFormatter<object>
    {
        public bool CanConvert(object argument, Type type)
        {
            return true;
        }

        public object Parse(string message)
        {
            throw new NotImplementedException();
        }

        public string Format(object message)
        {
            throw new NotImplementedException();
        }

        public TTarget Deserialize<TTarget>(object message)
        {
            return (TTarget) message;
        }

        public object Deserialize(Type type, object message)
        {
            return message;
        }

        public object Serialize(object value)
        {
            return value;
        }
    }

    public class MockWampServerProxyFactory<TMessage> : IWampServerProxyFactory<TMessage>
    {
        private readonly IWampServer mServer;
        private IWampRpcClient<TMessage> mClient;

        public MockWampServerProxyFactory(IWampServer server)
        {
            mServer = server;
        }

        public IWampRpcClient<TMessage> Client
        {
            get
            {
                return mClient;
            }
        }

        public IWampServer Create(IWampRpcClient<TMessage> client, IWampConnection<TMessage> connection)
        {
            mClient = client;
            return mServer;
        }
    }


    public interface ICalculator
    {
        int Square(int number);
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

        public Task<object> HandleAsync(WampRpcCall<object> rpcCall)
        {
            LastMessage = rpcCall;

            return new Task<object>(() => mResult);
        }
    }

    class MockWampRpcClientHandlerBuilder<TMessage> : IWampRpcClientHandlerBuilder<TMessage>
    {
        private readonly MockWampRpcClientHandler mHandler;

        public MockWampRpcClientHandlerBuilder(MockWampRpcClientHandler handler)
        {
            mHandler = handler;
        }

        public IWampRpcClientHandler Build(IWampConnection<TMessage> connection)
        {
            return mHandler;
        }
    }
}