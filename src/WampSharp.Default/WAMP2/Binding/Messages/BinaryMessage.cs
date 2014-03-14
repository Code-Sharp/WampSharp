using WampSharp.Core.Message;

namespace WampSharp.Binding
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