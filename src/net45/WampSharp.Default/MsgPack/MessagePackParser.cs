using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Msgpack;
using WampSharp.Core.Message;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.Msgpack
{
    public class MessagePackParser : IWampBinaryMessageParser<JToken>
    {
        private readonly JsonWampMessageFormatter mMessageFormatter;

        public MessagePackParser()
        {
            mMessageFormatter = new JsonWampMessageFormatter();
        }

        public WampMessage<JToken> Parse(byte[] raw)
        {
            using (MemoryStream memoryStream = new MemoryStream(raw, false))
            {
                using (MessagePackReader reader = new MessagePackReader(memoryStream))
                {
                    JToken token = JToken.Load(reader);
                    WampMessage<JToken> message = mMessageFormatter.Parse(token);
                    return message;
                }
            }
        }

        public byte[] Format(WampMessage<JToken> message)
        {
            JToken formatted = mMessageFormatter.Format(message);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (MessagePackWriter writer = new MessagePackWriter(memoryStream))
                {
                    formatted.WriteTo(writer);
                    memoryStream.Position = 0;
                    byte[] result = memoryStream.ToArray();
                    return result;
                }
            }
        }
    }
}