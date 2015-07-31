using System;
using System.IO;
using WampSharp.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Message;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.Newtonsoft
{
    public class JTokenMessageParser : IWampTextMessageParser<JToken>,
        IWampStreamingMessageParser<JToken>
    {
        private readonly JsonSerializer mSerializer;
        private readonly IWampMessageFormatter<JToken> mMessageFormatter;
        private readonly ILog mLogger;

        public JTokenMessageParser(JsonSerializer serializer)
        {
            mSerializer = serializer;
            mMessageFormatter = new JsonWampMessageFormatter();
            mLogger = LogProvider.GetLogger(this.GetType());
        }

        public WampMessage<JToken> Parse(string text)
        {
            try
            {
                mLogger.DebugFormat("Trying to parse message {0}", text);
                return mMessageFormatter.Parse(JToken.Parse(text));
            }
            catch (Exception ex)
            {
                mLogger.ErrorFormat(ex, "Failed parsing message {0}", text);
                throw;
            }
        }

        public string Format(WampMessage<object> message)
        {
            StringWriter writer = new StringWriter();
            JsonTextWriter jsonWriter = new JsonTextWriter(writer);
            jsonWriter.Formatting = Formatting.None;
            object[] array = mMessageFormatter.Format(message);
            mSerializer.Serialize(jsonWriter, array);
            string result = writer.ToString();
            
            mLogger.DebugFormat("Formatted message {0}", result);
            return result;
        }

        public WampMessage<JToken> Parse(Stream stream)
        {
            try
            {
                using (JsonReader reader = new JsonTextReader(new StreamReader(stream)) {CloseInput = false})
                {
                    JToken parsed = JToken.ReadFrom(reader);
                    return mMessageFormatter.Parse(parsed);
                }
            }
            catch (Exception ex)
            {
                mLogger.ErrorFormat(ex, "Failed parsing message.");
                throw;
            }
        }

        public void Format(WampMessage<object> message, Stream stream)
        {
            using (JsonTextWriter textWriter = new JsonTextWriter(new StreamWriter(stream)) {CloseOutput = false})
            {
                textWriter.Formatting = Formatting.None;
                object[] array = mMessageFormatter.Format(message);
                mSerializer.Serialize(textWriter, array);
                textWriter.Flush();
            }
        }
    }
}