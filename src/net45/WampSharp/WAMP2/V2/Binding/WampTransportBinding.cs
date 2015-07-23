using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding.Messages;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.V2.Binding
{
    /// <summary>
    /// A base class for a WAMP transport binding.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TRaw"></typeparam>
    public abstract class WampTransportBinding<TMessage, TRaw> : WampBinding<TMessage>, 
        IWampTransportBinding<TMessage, TRaw>
    {
        private readonly IWampMessageParser<TMessage, TRaw> mParser;

        protected WampTransportBinding(IWampFormatter<TMessage> formatter, IWampMessageParser<TMessage, TRaw> parser, string protocolName)
            :base(protocolName, formatter)
        {
            mParser = parser;
        }

        public WampMessage<TMessage> Parse(TRaw message)
        {
            return mParser.Parse(message);
        }

        public TRaw Format(WampMessage<object> message)
        {
            RawMessage<TRaw> textMessage = GetFormattedMessage(message);
            return textMessage.Raw;
        }

        public override WampMessage<object> GetRawMessage(WampMessage<object> message)
        {
            return GetFormattedMessage(message);
        }

        private RawMessage<TRaw> GetFormattedMessage(WampMessage<object> message)
        {
            RawMessage<TRaw> result = message as RawMessage<TRaw>;

            if (result == null)
            {
                result = new RawMessage<TRaw>(message);
                result.Raw = mParser.Format(message);
            }

            return result;
        }

        public WampMessage<TMessage> Parse(byte[] bytes, int position, int length)
        {
            return mParser.Parse(bytes, position, length);
        }

        public int Format(WampMessage<object> message, byte[] bytes, int position)
        {
            // TODO: You know! reuse the RawMessage if possible :)
            return mParser.Format(message, bytes, position);
        }
    }
}