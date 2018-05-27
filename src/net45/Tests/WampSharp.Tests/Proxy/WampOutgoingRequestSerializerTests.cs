using NUnit.Framework;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests.Proxy
{
    [TestFixture]
    public class WampOutgoingRequestSerializerTests
    {
        private WampOutgoingRequestSerializer<MockRaw> mOutgoingRequestSerializer;
        private WampMessageEqualityComparer<MockRaw> mComparer;
        private readonly IWampFormatter<MockRaw> mFormatter = new MockRawFormatter();

        [SetUp]
        public void Setup()
        {
            mOutgoingRequestSerializer = new WampOutgoingRequestSerializer<MockRaw>(new MockRawFormatter());

            mComparer =
                new WampMessageEqualityComparer<MockRaw>
                    (new MockRawComparer());
        }

        #region Tests

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.WelcomeMessages))]
        public void Welcome(string sessionId, int protocolVersion, string serverIdent)
        {
            WampMessage<MockRaw> serialized =
                SerializeWelcome(sessionId, protocolVersion, serverIdent);

            WampMessage<MockRaw> raw = 
                WampV1Messages.Welcome(sessionId, protocolVersion, serverIdent);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.PrefixMessages))]
        public void Prefix(string prefix, string uri)
        {
            WampMessage<MockRaw> serialized =
                SerializePrefix(prefix, uri);

            WampMessage<MockRaw> raw =
                WampV1Messages.Prefix(prefix, uri);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.CallMessages))]
        public void Call(string callId, string procUri, object[] arguments)
        {
            WampMessage<MockRaw> serialized =
                SerializeCall(callId, procUri, arguments);

            WampMessage<MockRaw> raw =
                WampV1Messages.Call(callId, procUri, arguments);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.CallErrorMessagesSimple))]
        public void CallError(string callId, string errorUri, string errorDesc)
        {
            WampMessage<MockRaw> serialized =
                SerializeCallError(callId, errorUri, errorDesc);

            WampMessage<MockRaw> raw =
                WampV1Messages.CallError(callId, errorUri, errorDesc);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.CallErrorMessagesDetailed))]
        public void CallError(string callId, string errorUri, string errorDesc, object errorDetails)
        {
            WampMessage<MockRaw> serialized =
                SerializeCallError(callId, errorUri, errorDesc, errorDetails);

            WampMessage<MockRaw> raw =
                WampV1Messages.CallError(callId, errorUri, errorDesc, errorDetails);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.CallResultMessages))]
        public void CallResult(string callId, object result)
        {
            WampMessage<MockRaw> serialized =
                SerializeCallResult(callId, result);

            WampMessage<MockRaw> raw =
                WampV1Messages.CallResult(callId, result);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.SubscribeMessages))]
        public void Subscribe(string topicUri)
        {
            WampMessage<MockRaw> serialized =
                SerializeSubscribe(topicUri);

            WampMessage<MockRaw> raw =
                WampV1Messages.Subscribe(topicUri);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.UnsubscribeMessages))]
        public void Unsubscribe(string topicUri)
        {
            WampMessage<MockRaw> serialized =
                SerializeUnsubscribe(topicUri);

            WampMessage<MockRaw> raw =
                WampV1Messages.Unsubscribe(topicUri);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }


        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.PublishMessagesSimple))]
        public void Publish(string topicUri, object @event)
        {
            WampMessage<MockRaw> serialized =
                SerializePublish(topicUri, @event);

            WampMessage<MockRaw> raw =
                WampV1Messages.Publish(topicUri, @event);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.PublishMessagesExcludeMe))]
        public void Publish(string topicUri, object @event, bool excludeMe)
        {
            WampMessage<MockRaw> serialized =
                SerializePublish(topicUri, @event, excludeMe);

            WampMessage<MockRaw> raw =
                WampV1Messages.Publish(topicUri, @event, excludeMe);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.PublishMessagesExclude))]
        public void Publish(string topicUri, object @event, string[] exclude)
        {
            WampMessage<MockRaw> serialized =
                SerializePublish(topicUri, @event, exclude);

            WampMessage<MockRaw> raw =
                WampV1Messages.Publish(topicUri, @event, exclude);

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }

        [TestCaseSource(typeof(MessagesArguments), nameof(MessagesArguments.PublishMessagesEligible))]
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
            var message =
                mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.Welcome(default(string), default(int), default(string))),
                 new object[] {sessionId, protocolVersion, serverIdent});

            return mFormatter.SerializeMessage(message);
        }

        public WampMessage<MockRaw> SerializeCallResult(string callId, object result)
        {
            var message = 
                mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                    client.CallResult(default(string), default(object))),
                    new object[] {callId, result});
            
            return mFormatter.SerializeMessage(message);
        }

        public WampMessage<MockRaw> SerializeCallError(string callId, string errorUri, string errorDesc)
        {
            var message =
                mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.CallError(default(string), default(string), default(string))),
                 new object[] {callId, errorUri, errorDesc});

            return mFormatter.SerializeMessage(message);
        }

        public WampMessage<MockRaw> SerializeCallError(string callId, string errorUri, string errorDesc, object errorDetails)
        {
            var message =
                mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.CallError(default(string), default(string), default(string), default(object))),
                 new object[] {callId, errorUri, errorDesc, errorDetails});

            return mFormatter.SerializeMessage(message);
        }

        public WampMessage<MockRaw> SerializeEvent(string topicUri, object @event)
        {
            var message =
                mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.Event(default(string), default(object))),
                 new object[] {topicUri, @event});

            return mFormatter.SerializeMessage(message);
        }

        public WampMessage<MockRaw> SerializePrefix(string prefix, string uri)
        {
            var message =
                mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Prefix(default(IWampClient), default(string), default(string))),
                 new object[] {default(IWampClient), prefix, uri});

            return mFormatter.SerializeMessage(message);
        }

        public WampMessage<MockRaw> SerializeCall(string callId, string procUri, params object[] arguments)
        {
            var message = mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Call(default(IWampClient), default(string), default(string), default(object[]))),
                 new object[] {default(IWampClient), callId, procUri, arguments});

            return mFormatter.SerializeMessage(message);
        }

        public WampMessage<MockRaw> SerializeSubscribe(string topicUri)
        {
            var message = mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Subscribe(default(IWampClient), default(string))),
                 new object[] {default(IWampClient), topicUri});

            return mFormatter.SerializeMessage(message);
        }

        public WampMessage<MockRaw> SerializeUnsubscribe(string topicUri)
        {
            var message = mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Unsubscribe(default(IWampClient), default(string))),
                 new object[] {default(IWampClient), topicUri});

            return mFormatter.SerializeMessage(message);
        }

        public WampMessage<MockRaw> SerializePublish(string topicUri, object @event)
        {
            var message = mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object))),
                 new object[] {default(IWampClient), topicUri, @event});
            
            return mFormatter.SerializeMessage(message);        
        }

        public WampMessage<MockRaw> SerializePublish(string topicUri, object @event, bool excludeMe)
        {
            var message = mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object), default(bool))),
                 new object[] {default(IWampClient), topicUri, @event, excludeMe});

            return mFormatter.SerializeMessage(message);
        }

        public WampMessage<MockRaw> SerializePublish(string topicUri, object @event, string[] exclude)
        {
            var message = mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object), default(string[]))),
                 new object[] {default(IWampClient), topicUri, @event, exclude});
            
            return mFormatter.SerializeMessage(message);
        }

        public WampMessage<MockRaw> SerializePublish(string topicUri, object @event, string[] exclude,
                                            string[] eligible)
        {
            var message = mOutgoingRequestSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object), default(string[]),
                                           default(string[]))),
                 new object[] {default(IWampClient), topicUri, @event, exclude, eligible});

            return mFormatter.SerializeMessage(message);
        }
        
        #endregion
    }
}