using System.Linq;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

namespace WampSharp.Tests.TestHelpers
{
    public static class WampFormatterExtensions
    {
        public static WampMessage<TMessage> SerializeMessage<TMessage>(this IWampFormatter<TMessage> formatter, WampMessage<object> message)
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