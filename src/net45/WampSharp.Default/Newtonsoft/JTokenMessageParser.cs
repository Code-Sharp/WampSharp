using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Message;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.Newtonsoft
{
    public class JTokenMessageParser : IWampTextMessageParser<JToken>
    {
        private readonly JsonSerializer mSerializer;
        private readonly IWampMessageFormatter<JToken> mMessageFormatter;

        public JTokenMessageParser(JsonSerializer serializer)
        {
            mSerializer = serializer;
            mMessageFormatter = new JsonWampMessageFormatter();
        }

        public WampMessage<JToken> Parse(string text)
        {
            return mMessageFormatter.Parse(JToken.Parse(text));
        }

        public string Format(WampMessage<object> message)
        {
            StringWriter writer = new StringWriter();
            JsonTextWriter jsonWriter = new JsonTextWriter(writer);
            jsonWriter.Formatting = Formatting.None;
            object[] array = mMessageFormatter.Format(message);
            mSerializer.Serialize(jsonWriter, array);
            return writer.ToString();
        }
    }
}