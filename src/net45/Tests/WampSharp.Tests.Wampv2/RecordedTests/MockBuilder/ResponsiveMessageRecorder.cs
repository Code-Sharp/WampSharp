using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    class ResponsiveMessageRecorder : MessageRecorder<MockRaw>
    {
        private readonly IEnumerable<WampMessage<MockRaw>> mCalls;
        private readonly Dictionary<WampMessageType, string> mMessageTypeToParameter;

        private readonly IWampRequestMapper<MockRaw> mRequestMapper =
            new RequestMapper();

        public ResponsiveMessageRecorder(IEnumerable<WampMessage<MockRaw>> calls,
                                   Dictionary<WampMessageType, string> messageTypeToParameter)
        {
            mCalls = calls;
            mMessageTypeToParameter = messageTypeToParameter;
        }

        public override void Record(WampMessage<MockRaw> message)
        {
            base.Record(message);


            if (mMessageTypeToParameter.TryGetValue(message.MessageType, out string argumentName))
            {
                int relevantIndex =
                    GetIndex(message, argumentName).Value;

                long? newId = GetArgumentValue(message, relevantIndex);

                long? requestId = message.GetRequestId();

                WampMessage<MockRaw> originalCall =
                    mCalls.FirstOrDefault(x => x.GetRequestId() == requestId &&
                                               x.MessageType == message.MessageType);

                long? originalId = GetArgumentValue(originalCall, relevantIndex);

                foreach (WampMessage<MockRaw> call in mCalls)
                {
                    int? currentIndex =
                        GetIndex(call, argumentName);

                    if (currentIndex != null)
                    {
                        long? current = GetArgumentValue(call, currentIndex.Value);

                        if (current == originalId)
                        {
                            call.Arguments[currentIndex.Value] = new MockRaw(newId);
                        }
                    }
                }
            }
        }

        private static long? GetArgumentValue(WampMessage<MockRaw> message, int relevantIndex)
        {
            return message.Arguments[relevantIndex].Value as long?;
        }

        private int? GetIndex(WampMessage<MockRaw> message, string argumentName)
        {
            var argumentWithIndex =
                mRequestMapper.Map(message).Method.GetParameters()
                              .Select((parameter, index) => new {parameter, index})
                              .FirstOrDefault(x => x.parameter.Name == argumentName);

            if (argumentWithIndex != null)
            {
                return argumentWithIndex.index;
            }

            return null;
        }
    }
}