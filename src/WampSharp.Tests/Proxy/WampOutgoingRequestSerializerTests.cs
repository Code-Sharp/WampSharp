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

        [TestCaseSource(typeof(MessagesArguments), "WelcomeMessages")]
        public void Welcome(string sessionId, int protocolVersion, string serverIdent)
        {
            WampMessage<MockRaw> serialized =
                SerializeWelcome(sessionId, protocolVersion, serverIdent);

            WampMessage<MockRaw> raw = 
                WampV1Messages.Welcome(sessionId, protocolVersion, serverIdent);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), "PrefixMessages")]
        public void Prefix(string prefix, string uri)
        {
            WampMessage<MockRaw> serialized =
                SerializePrefix(prefix, uri);

            WampMessage<MockRaw> raw =
                WampV1Messages.Prefix(prefix, uri);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), "CallMessages")]
        public void Call(string callId, string procUri, object[] arguments)
        {
            WampMessage<MockRaw> serialized =
                SerializeCall(callId, procUri, arguments);

            WampMessage<MockRaw> raw =
                WampV1Messages.Call(callId, procUri, arguments);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), "CallErrorMessagesSimple")]
        public void CallError(string callId, string errorUri, string errorDesc)
        {
            WampMessage<MockRaw> serialized =
                SerializeCallError(callId, errorUri, errorDesc);

            WampMessage<MockRaw> raw =
                WampV1Messages.CallError(callId, errorUri, errorDesc);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), "CallErrorMessagesDetailed")]
        public void CallError(string callId, string errorUri, string errorDesc, object errorDetails)
        {
            WampMessage<MockRaw> serialized =
                SerializeCallError(callId, errorUri, errorDesc, errorDetails);

            WampMessage<MockRaw> raw =
                WampV1Messages.CallError(callId, errorUri, errorDesc, errorDetails);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), "CallResultMessages")]
        public void CallResult(string callId, object result)
        {
            WampMessage<MockRaw> serialized =
                SerializeCallResult(callId, result);

            WampMessage<MockRaw> raw =
                WampV1Messages.CallResult(callId, result);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), "SubscribeMessages")]
        public void Subscribe(string topicUri)
        {
            WampMessage<MockRaw> serialized =
                SerializeSubscribe(topicUri);

            WampMessage<MockRaw> raw =
                WampV1Messages.Subscribe(topicUri);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), "UnsubscribeMessages")]
        public void Unsubscribe(string topicUri)
        {
            WampMessage<MockRaw> serialized =
                SerializeUnsubscribe(topicUri);

            WampMessage<MockRaw> raw =
                WampV1Messages.Unsubscribe(topicUri);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }


        [TestCaseSource(typeof(MessagesArguments), "PublishMessagesSimple")]
        public void Publish(string topicUri, object @event)
        {
            WampMessage<MockRaw> serialized =
                SerializePublish(topicUri, @event);

            WampMessage<MockRaw> raw =
                WampV1Messages.Publish(topicUri, @event);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), "PublishMessagesExcludeMe")]
        public void Publish(string topicUri, object @event, bool excludeMe)
        {
            WampMessage<MockRaw> serialized =
                SerializePublish(topicUri, @event, excludeMe);

            WampMessage<MockRaw> raw =
                WampV1Messages.Publish(topicUri, @event, excludeMe);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), "PublishMessagesExclude")]
        public void Publish(string topicUri, object @event, string[] exclude)
        {
            WampMessage<MockRaw> serialized =
                SerializePublish(topicUri, @event, exclude);

            WampMessage<MockRaw> raw =
                WampV1Messages.Publish(topicUri, @event, exclude);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), "PublishMessagesEligible")]
        public void Publish(string topicUri, object @event, string[] exclude, string[] eligible)
        {
            WampMessage<MockRaw> serialized =
                SerializePublish(topicUri, @event, exclude, eligible);

            WampMessage<MockRaw> raw =
                WampV1Messages.Publish(topicUri, @event, exclude, eligible);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        #endregion

        #region Helper Methods

        public WampMessage<MockRaw> SerializeWelcome(string sessionId, int protocolVersion, string serverIdent)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.Welcome(default(string), default(int), default(string))),
                 new object[] {sessionId, protocolVersion, serverIdent});
        }

        public WampMessage<MockRaw> SerializeCallResult(string callId, object result)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.CallResult(default(string), default(object))),
                 new object[] {callId, result});
        }

        public WampMessage<MockRaw> SerializeCallError(string callId, string errorUri, string errorDesc)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.CallError(default(string), default(string), default(string))),
                 new object[] {callId, errorUri, errorDesc});
        }

        public WampMessage<MockRaw> SerializeCallError(string callId, string errorUri, string errorDesc, object errorDetails)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.CallError(default(string), default(string), default(string), default(object))),
                 new object[] {callId, errorUri, errorDesc, errorDetails});
        }

        public WampMessage<MockRaw> SerializeEvent(string topicUri, object @event)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.Event(default(string), default(object))),
                 new object[] {topicUri, @event});
        }

        public WampMessage<MockRaw> SerializePrefix(string prefix, string uri)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Prefix(default(IWampClient), default(string), default(string))),
                 new object[] {default(IWampClient), prefix, uri});
        }

        public WampMessage<MockRaw> SerializeCall(string callId, string procUri, params object[] arguments)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Call(default(IWampClient), default(string), default(string), default(object[]))),
                 new object[] {default(IWampClient), callId, procUri, arguments});
        }

        public WampMessage<MockRaw> SerializeSubscribe(string topicUri)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Subscribe(default(IWampClient), default(string))),
                 new object[] {default(IWampClient), topicUri});
        }

        public WampMessage<MockRaw> SerializeUnsubscribe(string topicUri)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Unsubscribe(default(IWampClient), default(string))),
                 new object[] {default(IWampClient), topicUri});
        }

        public WampMessage<MockRaw> SerializePublish(string topicUri, object @event)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object))),
                 new object[] {default(IWampClient), topicUri, @event});
        }

        public WampMessage<MockRaw> SerializePublish(string topicUri, object @event, bool excludeMe)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object), default(bool))),
                 new object[] {default(IWampClient), topicUri, @event, excludeMe});
        }

        public WampMessage<MockRaw> SerializePublish(string topicUri, object @event, string[] exclude)
        {
            return mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object), default(string[]))),
                 new object[] {default(IWampClient), topicUri, @event, exclude});
        }

        public WampMessage<MockRaw> SerializePublish(string topicUri, object @event, string[] exclude,
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