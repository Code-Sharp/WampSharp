using System.Collections.Generic;
using WampSharp.Core.Message;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public class MessageRecorder<TMessage> : IMessageRecorder<TMessage>
    {
        private readonly ICollection<WampMessage<TMessage>> mRecordedMessages =
            new List<WampMessage<TMessage>>();

        public MessageRecorder()
        {
        }

        public virtual void Record(WampMessage<TMessage> message)
        {
            mRecordedMessages.Add(message);
        }

        public IEnumerable<WampMessage<TMessage>> RecordedMessages => mRecordedMessages;
    }
}