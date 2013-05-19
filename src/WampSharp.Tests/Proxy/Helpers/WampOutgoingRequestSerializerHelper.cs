using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Proxy.Helpers
{
    public class WampOutgoingRequestSerializerHelper
    {
        private readonly IWampOutgoingRequestSerializer<MockRaw> mSerializer;

        public WampOutgoingRequestSerializerHelper(IWampOutgoingRequestSerializer<MockRaw> serializer)
        {
            mSerializer = serializer;
        }

        public WampMessage<MockRaw> Welcome(string sessionId, int protocolVersion, string serverIdent)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.Welcome(default(string), default(int), default(string))),
                 new object[] {sessionId, protocolVersion, serverIdent});
        }

        public WampMessage<MockRaw> CallResult(string callId, object result)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.CallResult(default(string), default(object))),
                 new object[] {callId, result});
        }

        public WampMessage<MockRaw> CallError(string callId, string errorUri, string errorDesc)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.CallError(default(string), default(string), default(string))),
                 new object[] {callId, errorUri, errorDesc});
        }

        public WampMessage<MockRaw> CallError(string callId, string errorUri, string errorDesc, object errorDetails)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.CallError(default(string), default(string), default(string), default(object))),
                 new object[] {callId, errorUri, errorDesc, errorDetails});
        }

        public WampMessage<MockRaw> Event(string topicUri, object @event)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampClient client) =>
                            client.Event(default(string), default(object))),
                 new object[] {topicUri, @event});
        }

        public WampMessage<MockRaw> Prefix(string prefix, string uri)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Prefix(default(IWampClient), default(string), default(string))),
                 new object[] {default(IWampClient), prefix, uri});
        }

        public WampMessage<MockRaw> Call(string callId, string procUri, params object[] arguments)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Call(default(IWampClient), default(string), default(string), default(object[]))),
                 new object[] {default(IWampClient), callId, procUri, arguments});
        }

        public WampMessage<MockRaw> Subscribe(string topicUri)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Subscribe(default(IWampClient), default(string))),
                 new object[] {default(IWampClient), topicUri});
        }

        public WampMessage<MockRaw> Unsubscribe(string topicUri)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Unsubscribe(default(IWampClient), default(string))),
                 new object[] {default(IWampClient), topicUri});
        }

        public WampMessage<MockRaw> Publish(string topicUri, object @event)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object))),
                 new object[] {default(IWampClient), topicUri, @event});
        }

        public WampMessage<MockRaw> Publish(string topicUri, object @event, bool excludeMe)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object), default(bool))),
                 new object[] {default(IWampClient), topicUri, @event, excludeMe});
        }

        public WampMessage<MockRaw> Publish(IWampClient client, string topicUri, object @event, string[] exclude)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object), default(string[]))),
                 new object[] {default(IWampClient), topicUri, @event, exclude});
        }

        public WampMessage<MockRaw> Publish(IWampClient client, string topicUri, object @event, string[] exclude,
                                            string[] eligible)
        {
            return mSerializer.SerializeRequest
                (Method.Get((IWampServer server) =>
                            server.Publish(default(IWampClient), default(string), default(object), default(string[]),
                                           default(string[]))),
                 new object[] {default(IWampClient), topicUri, @event, exclude, eligible});
        }
    }
}