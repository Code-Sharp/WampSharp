using WampSharp.Core.Message;

namespace WampSharp.V2.Binding.Messages
{
    internal class BinaryMessage<TMessage> : WampMessage<TMessage>
    {
        public BinaryMessage(WampMessage<TMessage> message) :
            base(message)
        {
        }

        public byte[] Bytes { get; set; }
    }
}