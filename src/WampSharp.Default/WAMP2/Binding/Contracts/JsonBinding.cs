using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Listener;

namespace WampSharp.Binding
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

        public override WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message)
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