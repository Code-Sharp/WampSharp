using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public class CalleeMessagePlayer : MessagePlayer<MockRaw>
    {
        private readonly MessageMapper mMapper = new MessageMapper();
        
        public CalleeMessagePlayer(IEnumerable<WampMessage<MockRaw>> messages, WampMessageType[] categories, IWampIncomingMessageHandler<MockRaw, IWampClientProxy<MockRaw>> handler)
            : base(messages, categories, handler)
        {
        }

        protected override IEnumerable<WampMessage<MockRaw>> FindResponses(WampMessage<MockRaw> message,
                                                                           WampMessage<MockRaw> request)
        {
            long? id = message.GetRequestId();
         
            long? recordedId = request.GetRequestId();

            WampMessage<MockRaw>[] messages =
                mMessages.Where(x => x.GetRequestId() == recordedId &&
                                     x.MessageType != request.MessageType)
                         .ToArray();

            foreach (WampMessage<MockRaw> currentMessage in messages)
            {
                SetRequestId(currentMessage, id);
            }

            return messages;
        }

        private void SetRequestId(WampMessage<MockRaw> wampMessage, long? value)
        {
            if (wampMessage.MessageType == WampMessageType.v2Error)
            {
                wampMessage.Arguments[1] = new MockRaw(value);
            }
            else if (wampMessage.Arguments.Length >= 1)
            {
                wampMessage.Arguments[0] = new MockRaw(value);
            }
        }

        protected override WampMessage<MockRaw> FindRequest(WampMessage<MockRaw> message)
        {
            return mMapper.MapRequest(message, mMessages, true);
        }
    }
}