using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding.Messages;
using WampSharp.V2.Binding.Parsers;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.Binding.Contracts
{
    public abstract class JsonBinding<TMessage> : WampBinding<TMessage>,
        IWampTextBinding<TMessage>
    {
        private readonly IWampTextMessageParser<TMessage> mParser;

        protected JsonBinding(IWampFormatter<TMessage> formatter, IWampTextMessageParser<TMessage> parser, string protocolName = "wamp.2.json")
            :base(protocolName, formatter)
        {
            mParser = parser;
        }

        public WampMessage<TMessage> Parse(string message)
        {
            return mParser.Parse(message);
        }

        public string Format(WampMessage<TMessage> message)
        {
            TextMessage<TMessage> textMessage = GetTextMessage(message);
            return textMessage.Text;
        }

        public override WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message)
        {
            return GetTextMessage(message);
        }

        private TextMessage<TMessage> GetTextMessage(WampMessage<TMessage> message)
        {
            TextMessage<TMessage> result = message as TextMessage<TMessage>;

            if (result == null)
            {
                result = new TextMessage<TMessage>(message);
                result.Text = mParser.Format(message);
            }

            return result;
        }
    }
}