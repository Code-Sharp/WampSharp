using WampSharp.Core.Message;

namespace WampSharp.V2.Binding.Messages
{
    internal class RawMessage<TRaw> : WampMessage<object>
    {
        public RawMessage(WampMessage<object> other) : base(other)
        {
        }

        public TRaw Raw { get; set; }

        public byte[] Bytes { get; set; }
    }
}