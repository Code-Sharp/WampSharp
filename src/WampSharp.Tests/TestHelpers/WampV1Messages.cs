using System.Linq;
using WampSharp.Core.Message;

namespace WampSharp.Tests.TestHelpers
{
    public static class WampV1Messages
    {
        public static WampMessage<MockRaw> Prefix(string prefix, string uri)
        {
            WampMessage<MockRaw> result = new WampMessage<MockRaw>();
            result.MessageType = WampMessageType.v1Prefix;
            result.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(prefix), 
                                       new MockRaw(uri) 
                                   };

            return result;
        }

        public static WampMessage<MockRaw> Call(string callId, string procUri, params object[] arguments)
        {
            WampMessage<MockRaw> result = new WampMessage<MockRaw>();
            result.MessageType = WampMessageType.v1Call;

            MockRaw[] rawArguments = new[]
                                         {
                                             new MockRaw(callId),
                                             new MockRaw(procUri)
                                         }
                .Concat(arguments.Select(x => new MockRaw(x)))
                .ToArray();

            result.Arguments = rawArguments;

            return result;
        }

        public static WampMessage<MockRaw> Subscribe(string topicUri)
        {
            WampMessage<MockRaw> result = new WampMessage<MockRaw>();
            result.MessageType = WampMessageType.v1Subscribe;
            result.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(topicUri)
                                   };

            return result;
        }

        public static WampMessage<MockRaw> Unsubscribe(string topicUri)
        {
            WampMessage<MockRaw> result = new WampMessage<MockRaw>();
            result.MessageType = WampMessageType.v1Unsubscribe;
            result.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(topicUri)
                                   };

            return result;
        }

        public static WampMessage<MockRaw> Publish(string topicUri, object @event)
        {
            WampMessage<MockRaw> result = new WampMessage<MockRaw>();
            result.MessageType = WampMessageType.v1Publish;
            result.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(topicUri),
                                       new MockRaw(@event), 
                                   };

            return result;
        }

        public static WampMessage<MockRaw> Publish(string topicUri, object @event, bool excludeMe)
        {
            WampMessage<MockRaw> result = new WampMessage<MockRaw>();
            result.MessageType = WampMessageType.v1Publish;
            result.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(topicUri),
                                       new MockRaw(@event), 
                                       new MockRaw(excludeMe), 
                                   };

            return result;
        }

        public static WampMessage<MockRaw> Publish(string topicUri, object @event, string[] exclude)
        {
            WampMessage<MockRaw> result = new WampMessage<MockRaw>();
            result.MessageType = WampMessageType.v1Publish;
            result.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(topicUri),
                                       new MockRaw(@event), 
                                       new MockRaw(exclude), 
                                   };

            return result;
        }

        public static WampMessage<MockRaw> Publish(string topicUri, object @event, string[] exclude, string[] eligible)
        {
            WampMessage<MockRaw> result = new WampMessage<MockRaw>();
            result.MessageType = WampMessageType.v1Publish;
            result.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(topicUri),
                                       new MockRaw(@event), 
                                       new MockRaw(exclude), 
                                       new MockRaw(eligible), 
                                   };

            return result;
        }

        public static WampMessage<MockRaw> Welcome(string sessionId, int protocolVersion, string serverIdent)
        {
            WampMessage<MockRaw> result = new WampMessage<MockRaw>();
            result.MessageType = WampMessageType.v1Welcome;
            result.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(sessionId),
                                       new MockRaw(protocolVersion), 
                                       new MockRaw(serverIdent), 
                                   };

            return result;
        }

        public static WampMessage<MockRaw> CallResult(string callId, object result)
        {
            WampMessage<MockRaw> wampResult = new WampMessage<MockRaw>();
            wampResult.MessageType = WampMessageType.v1CallResult;
            wampResult.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(callId),
                                       new MockRaw(result), 
                                   };

            return wampResult;
        }

        public static WampMessage<MockRaw> CallError(string callId, string errorUri, string errorDesc)
        {
            WampMessage<MockRaw> wampResult = new WampMessage<MockRaw>();
            wampResult.MessageType = WampMessageType.v1CallError;
            wampResult.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(callId),
                                       new MockRaw(errorUri), 
                                       new MockRaw(errorDesc), 
                                   };

            return wampResult;
        }

        public static WampMessage<MockRaw> CallError(string callId, string errorUri, string errorDesc, object errorDetails)
        {
            WampMessage<MockRaw> wampResult = new WampMessage<MockRaw>();
            wampResult.MessageType = WampMessageType.v1CallError;
            wampResult.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(callId),
                                       new MockRaw(errorUri), 
                                       new MockRaw(errorDesc),
                                       new MockRaw(errorDetails), 
                                   };

            return wampResult;
        }

        public static WampMessage<MockRaw> Event(string topicUri, object @event)
        {
            WampMessage<MockRaw> wampResult = new WampMessage<MockRaw>();
            wampResult.MessageType = WampMessageType.v1Event;
            wampResult.Arguments = new MockRaw[]
                                   {
                                       new MockRaw(topicUri),
                                       new MockRaw(@event)
                                   };

            return wampResult;
        }
    }
}