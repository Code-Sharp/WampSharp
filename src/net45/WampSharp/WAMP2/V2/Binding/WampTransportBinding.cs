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

        public TRaw Format(WampMessage<TMessage> message)
        {
            RawMessage<TMessage, TRaw> textMessage = GetFormattedMessage(message);
            return textMessage.Raw;
        }

        public override WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message)
        {
            return GetFormattedMessage(message);
        }

        private RawMessage<TMessage, TRaw> GetFormattedMessage(WampMessage<TMessage> message)
        {
            RawMessage<TMessage, TRaw> result = message as RawMessage<TMessage, TRaw>;

            if (result == null)
            {
                result = new RawMessage<TMessage, TRaw>(message);
                result.Raw = mParser.Format(message);
            }

            return result;
        }
    }
}