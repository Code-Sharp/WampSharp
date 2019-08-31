using WampSharp.Core.Message;
using WampSharp.V2.Binding.Messages;

namespace WampSharp.V2.Binding.Parsers
{
    public static class WampMessageParserExtensions
    {
        public static byte[] GetBytes<TMessage, TRaw>(this IWampMessageParser<TMessage, TRaw> parser,
                                                      WampMessage<object> message)
        {
            if (message is RawMessage<TRaw> rawMessage &&
                rawMessage.Bytes != null)
            {
                return rawMessage.Bytes;
            }
            else
            {
                TRaw raw = parser.Format(message);
                byte[] binary = parser.GetBytes(raw);
                return binary;
            }
        }
    }
}