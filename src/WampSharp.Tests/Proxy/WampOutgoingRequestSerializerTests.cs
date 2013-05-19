using NUnit.Framework;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Tests.Proxy.Helpers;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Proxy
{
    [TestFixture]
    public class WampOutgoingRequestSerializerTests
    {
        private WampOutgoingRequestSerializer<MockRaw> mOutgoingRequestSerializer;
        private WampMessageEqualityComparer<MockRaw> mComparer;

        [SetUp]
        public void Setup()
        {
            mOutgoingRequestSerializer = new WampOutgoingRequestSerializer<MockRaw>(new MockRawFormatter());

            mComparer =
                new WampMessageEqualityComparer<MockRaw>
                    (new MockRawComparer());
        }

        #region Tests

        [Test]
        public void Welcome()
        {
            WampMessage<MockRaw> serialized =
                Welcome("v59mbCGDXZ7WTyxB", 1, "Autobahn/0.5.1");

            WampMessage<MockRaw> raw = 
                WampV1Messages.Welcome("v59mbCGDXZ7WTyxB", 1, "Autobahn/0.5.1");

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        #endregion

        #region Helper Methods

        public WampMessage<MockRaw> Welcome(string sessionId, int protocolVersion, string serverIdent)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.Welcome(default(string), default(int), default(string))),
                 new object[] {sessionId, protocolVersion, serverIdent});
        }

        public WampMessage<MockRaw> CallResult(string callId, object result)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.CallResult(default(string), default(object))),
                 new object[] {callId, result});
        }

        public WampMessage<MockRaw> CallError(string callId, string errorUri, string errorDesc)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.CallError(default(string), default(string), default(string))),
                 new object[] {callId, errorUri, errorDesc});
        }

        public WampMessage<MockRaw> CallError(string callId, string errorUri, string errorDesc, object errorDetails)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.CallError(default(string), default(string), default(string), default(object))),
                 new object[] {callId, errorUri, errorDesc, errorDetails});
        }

        public WampMessage<MockRaw> Event(string topicUri, object @event)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.Event(default(string), default(object))),
                 new object[] {topicUri, @event});
        }

        public WampMessage<MockRaw> Prefix(string prefix, string uri)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Prefix(default(IWampClient), default(string), default(string))),
                 new object[] {default(IWampClient), prefix, uri});
        }

        public WampMessage<MockRaw> Call(string callId, string procUri, params object[] arguments)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Call(default(IWampClient), default(string), default(string), default(object[]))),
                 new object[] {default(IWampClient), callId, procUri, arguments});
        }

        public WampMessage<MockRaw> Subscribe(string topicUri)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Subscribe(default(IWampClient), default(string))),
                 new object[] {default(IWampClient), topicUri});
        }

        public WampMessage<MockRaw> Unsubscribe(string topicUri)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Unsubscribe(default(IWampClient), default(string))),
                 new object[] {default(IWampClient), topicUri});
        }

        public WampMessage<MockRaw> Publish(string topicUri, object @event)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object))),
                 new object[] {default(IWampClient), topicUri, @event});
        }

        public WampMessage<MockRaw> Publish(string topicUri, object @event, bool excludeMe)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object), default(bool))),
                 new object[] {default(IWampClient), topicUri, @event, excludeMe});
        }

        public WampMessage<MockRaw> Publish(string topicUri, object @event, string[] exclude)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object), default(string[]))),
                 new object[] {default(IWampClient), topicUri, @event, exclude});
        }

        public WampMessage<MockRaw> Publish(string topicUri, object @event, string[] exclude,
                                            string[] eligible)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object), default(string[]),
                                           default(string[]))),
                 new object[] {default(IWampClient), topicUri, @event, exclude, eligible});
        }
        
        #endregion
    }
}