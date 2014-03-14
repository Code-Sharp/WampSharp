using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Listener;

namespace WampSharp.Binding
{
    public abstract class MsgPackBinding<TMessage> : IWampBinaryBinding<TMessage>
    {
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly string mName;
        private readonly IWampBinaryMessageParser<TMessage> mParser;

        protected MsgPackBinding(IWampFormatter<TMessage> formatter, IWampBinaryMessageParser<TMessage> parser)
        {
            mFormatter = formatter;
            mParser = parser;
            mName = "wamp.2.msgpack";
        }

        public WampMessage<TMessage> Parse(byte[] bytes)
        {
            return mParser.Parse(bytes);
        }

        public string Name
        {
            get { return mName; }
        }

        public IWampFormatter<TMessage> Formatter
        {
            get { return mFormatter; }
        }

        public WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message)
        {
            BinaryMessage<TMessage> result = new BinaryMessage<TMessage>(message);
            result.Bytes = mParser.Format(message);
            return result;
        }
    }
}
