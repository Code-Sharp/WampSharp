using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.Wampv2.Binding;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.MockBuilder
{
    public class MessagePlayerImpl : MessagePlayer<MockRaw>
    {
        private readonly MessageMapper mMapper = new MessageMapper();
        
        public MessagePlayerImpl(ICollection<WampMessage<MockRaw>> messages, WampMessageType[] categories, IWampIncomingMessageHandler<MockRaw, IWampClient<MockRaw>> handler)
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
                // Yuck: ( 
                mMessages.Remove(currentMessage);
            }

            // Yuck: This request belongs only to the following test case.
            // Once we finished with it, we remove it, so other requests won't
            // get confused and think this is the request representing them.
            // There can be a better way doing this by finding registrations by
            // their registration ids, but I'm not going to do this soon. :(
            mMessages.Remove(request);

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