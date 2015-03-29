using System.Linq;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Message;

namespace WampSharp.Newtonsoft
{
    public class JsonWampMessageFormatter : IWampMessageFormatter<JToken>
    {
        public WampMessage<JToken> Parse(JToken message)
        {
            JArray array = message as JArray;

            return new WampMessage<JToken>()
                       {
                           Arguments = array.Skip(1).ToArray(),
                           MessageType = array[0].ToObject<WampMessageType>()
                       };
        }

        public object[] Format(WampMessage<object> message)
        {
            object[] content = new object[message.Arguments.Length + 1];
            content[0] = (int) message.MessageType;
            message.Arguments.CopyTo(content, 1);
            return content;
        }
    }
}