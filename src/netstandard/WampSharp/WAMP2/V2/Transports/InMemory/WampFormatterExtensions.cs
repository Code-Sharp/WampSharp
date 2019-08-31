using System.Linq;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.Transports
{
    internal static class WampFormatterExtensions
    {
        public static WampMessage<TMessage> SerializeMessage<TMessage>(this IWampFormatter<TMessage> formatter, WampMessage<object> message)
        {

            if (message is WampMessage<TMessage> casted)
            {
                return casted;
            }
            else
            {
                WampMessage<TMessage> result = new WampMessage<TMessage>();

                result.MessageType = message.MessageType;

                result.Arguments =
                    message.Arguments.Select(x => formatter.Serialize(x))
                        .ToArray();

                return result;
            }
        }
    }
}