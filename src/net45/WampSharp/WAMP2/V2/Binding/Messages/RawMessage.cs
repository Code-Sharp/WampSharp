using WampSharp.Core.Message;

namespace WampSharp.V2.Binding.Messages
{
    internal class RawMessage<TMessage, TRaw> : WampMessage<TMessage>
    {
        public RawMessage(WampMessage<TMessage> other) : base(other)
        {
        }

        public TRaw Raw { get; set; }
    }
}