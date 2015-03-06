using System;
using System.IO;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Logs;
using WampSharp.Core.Message;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.Newtonsoft
{
    public class JTokenMessageParser : IWampTextMessageParser<JToken>
    {
        private readonly IWampMessageFormatter<JToken> mMessageFormatter;

        private readonly ILogger mLogger;

        public JTokenMessageParser()
        {
            mMessageFormatter = new JsonWampMessageFormatter();
            mLogger = WampLoggerFactory.Create(this.GetType());
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

        public string Format(WampMessage<JToken> message)
        {
            StringWriter writer = new StringWriter();
            JsonTextWriter jsonWriter = new JsonTextWriter(writer);
            jsonWriter.Formatting = Formatting.None;
            JToken raw = mMessageFormatter.Format(message);
            raw.WriteTo(jsonWriter);
            string result = writer.ToString();
            
            mLogger.DebugFormat("Formatted message {0}", result);
            return result;
        }
    }
}