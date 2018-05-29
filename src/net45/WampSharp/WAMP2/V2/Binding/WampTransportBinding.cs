using System.IO;
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

        public byte[] GetBytes(TRaw raw)
        {
            return mParser.GetBytes(raw);
        }

        public override WampMessage<object> GetRawMessage(WampMessage<object> message)
        {
            return GetFormattedMessage(message);
        }

        private RawMessage<TRaw> GetFormattedMessage(WampMessage<object> message)
        {
            if (!(message is RawMessage<TRaw> result))
            {
                result = new RawMessage<TRaw>(message);
                result.Raw = mParser.Format(message);

                if (ComputeBytes == true)
                {
                    result.Bytes = mParser.GetBytes(result.Raw);
                }
            }

            return result;
        }

        public WampMessage<TMessage> Parse(Stream stream)
        {
            return mParser.Parse(stream);
        }

        public void Format(WampMessage<object> message, Stream stream)
        {
            if (ComputeBytes == true && 
                message is RawMessage<TRaw> casted)
            {
                byte[] bytes = casted.Bytes;
                stream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                mParser.Format(message, stream);
            }
        }

        public bool? ComputeBytes { get; set; }
    }
}