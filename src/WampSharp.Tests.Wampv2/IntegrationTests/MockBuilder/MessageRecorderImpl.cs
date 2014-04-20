﻿using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.MockBuilder
{
    class MessageRecorderImpl : MessageRecorder<MockRaw>
    {
        private readonly IEnumerable<WampMessage<MockRaw>> mCalls;
        private readonly Dictionary<WampMessageType, string> mMessageTypeToParameter;

        private readonly IWampRequestMapper<MockRaw> mRequestMapper =
            new WampRequestMapper<MockRaw>
                (typeof (IWampClient<>), new MockRawFormatter());

        public MessageRecorderImpl(IEnumerable<WampMessage<MockRaw>> calls,
                                   Dictionary<WampMessageType, string> messageTypeToParameter)
        {
            mCalls = calls;
            mMessageTypeToParameter = messageTypeToParameter;
        }

        public override void Record(WampMessage<MockRaw> message)
        {
            base.Record(message);

            string argumentName;

            if (mMessageTypeToParameter.TryGetValue(message.MessageType, out argumentName))
            {
                int relevantIndex =
                    GetIndex(message, argumentName);

                long? newId = GetArgumentValue(message, relevantIndex);

                long? requestId = message.GetRequestId();

                WampMessage<MockRaw> originalCall =
                    mCalls.FirstOrDefault(x => x.GetRequestId() == requestId &&
                                               x.MessageType == message.MessageType);

                long? originalId = GetArgumentValue(originalCall, relevantIndex);

                foreach (WampMessage<MockRaw> call in mCalls)
                {
                    int currentIndex =
                        GetIndex(message, argumentName);

                    long? current = GetArgumentValue(call, currentIndex);

                    if (current == originalId)
                    {
                        call.Arguments[currentIndex] = new MockRaw(newId);
                    }
                }
            }
        }

        private static long? GetArgumentValue(WampMessage<MockRaw> message, int relevantIndex)
        {
            return message.Arguments[relevantIndex].Value as long?;
        }

        private int GetIndex(WampMessage<MockRaw> message, string argumentName)
        {
            return mRequestMapper.Map(message).Method.GetParameters()
                                 .Select((parameter, index) => new {parameter, index})
                                 .FirstOrDefault(x => x.parameter.Name == argumentName)
                                 .index;
        }
    }
}