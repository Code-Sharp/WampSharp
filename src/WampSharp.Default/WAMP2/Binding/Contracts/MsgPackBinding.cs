using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Listener;

namespace WampSharp.Binding
{
    public abstract class MsgPackBinding<TMessage> : WampBinding<TMessage>,
        IWampBinaryBinding<TMessage>
    {
        private readonly IWampBinaryMessageParser<TMessage> mParser;

        protected MsgPackBinding(IWampFormatter<TMessage> formatter, IWampBinaryMessageParser<TMessage> parser)
            : base("wamp.2.msgpack", formatter)
        {
            mParser = parser;
        }

        public WampMessage<TMessage> Parse(byte[] bytes)
        {
            return mParser.Parse(bytes);
        }

        public override WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message)
        {
            BinaryMessage<TMessage> result = message as BinaryMessage<TMessage>;

            if (result == null)
            {
                result = new BinaryMessage<TMessage>(message);
                result.Bytes = mParser.Format(message);
            }

            return result;
        }
    }
}
