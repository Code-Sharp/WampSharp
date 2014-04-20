using System.Collections.Generic;
using WampSharp.Core.Message;

namespace WampSharp.Tests.Wampv2.MockBuilder
{
    public interface IMessageRecorder<TMessage>
    {
        void Record(WampMessage<TMessage> message);

        IEnumerable<WampMessage<TMessage>> RecordedMessages { get; }
    }
}