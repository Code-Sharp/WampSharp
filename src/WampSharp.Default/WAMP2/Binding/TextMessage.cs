using WampSharp.Core.Message;

namespace WampSharp.Binding
{
    internal class TextMessage<TMessage> : WampMessage<TMessage>
    {
        public TextMessage(WampMessage<TMessage> message) :
            base(message)
        {
        }

        public string Text { get; set; }
    }
}