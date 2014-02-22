using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Listener;

namespace WampSharp.Binding
{
    public abstract class JsonBinding<TMessage> : IWampBinding<TMessage>
    {
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly string mName;
        private readonly IWampMessageParser<TMessage> mParser;

        protected JsonBinding(IWampFormatter<TMessage> formatter, IWampMessageParser<TMessage> parser)
        {
            mFormatter = formatter;
            mParser = parser;
            mName = "wamp.2.json";
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