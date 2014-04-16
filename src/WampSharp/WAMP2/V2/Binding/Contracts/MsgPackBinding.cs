using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding.Messages;
using WampSharp.V2.Binding.Parsers;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.Binding.Contracts
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

        public byte[] Format(WampMessage<TMessage> message)
        {
            BinaryMessage<TMessage> binaryMessage = GetBinaryMessage(message);
            return binaryMessage.Bytes;
        }

        public override WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message)
        {
            return GetBinaryMessage(message);
        }

        private BinaryMessage<TMessage> GetBinaryMessage(WampMessage<TMessage> message)
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
