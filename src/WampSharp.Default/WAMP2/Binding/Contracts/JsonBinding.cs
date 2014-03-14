using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Listener;

namespace WampSharp.Binding
{
    public abstract class JsonBinding<TMessage> : IWampTextBinding<TMessage>
    {
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly string mName;
        private readonly IWampTextMessageParser<TMessage> mParser;

        protected JsonBinding(IWampFormatter<TMessage> formatter, IWampTextMessageParser<TMessage> parser, string protocolName = "wamp.2.json")
        {
            mFormatter = formatter;
            mParser = parser;
            mName = protocolName;
        }

        public WampMessage<TMessage> Parse(string message)
        {
            return mParser.Parse(message);
        }

        public string Name
        {
            get
            {
                return mName;
            }
        }

        public IWampFormatter<TMessage> Formatter
        {
            get
            {
                return mFormatter;
            }
        }

        public WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message)
        {
            TextMessage<TMessage> result = new TextMessage<TMessage>(message);
            result.Text = mParser.Format(message);
            return result;
        }
    }
}