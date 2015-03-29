using System;
using System.IO;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Msgpack;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Logs;
using WampSharp.Core.Message;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.Msgpack
{
    public class MessagePackParser : IWampBinaryMessageParser<JToken>
    {
        private readonly JsonSerializer mSerializer;
        private readonly ILogger mLogger;
        private readonly JsonWampMessageFormatter mMessageFormatter;

        public MessagePackParser(JsonSerializer serializer)
        {
            mSerializer = serializer;
            mMessageFormatter = new JsonWampMessageFormatter();
            mLogger = WampLoggerFactory.Create(this.GetType());
        }

        public WampMessage<JToken> Parse(byte[] raw)
        {
            using (MemoryStream memoryStream = new MemoryStream(raw, false))
            {
                using (MessagePackReader reader = new MessagePackReader(memoryStream))
                {
                    try
                    {
                        mLogger.Debug(() => string.Format("Trying to parse msgpack message: {0}",
                            Convert.ToBase64String(raw)));

                        JToken token = JToken.Load(reader);

                        mLogger.Debug(() => string.Format("Parsed msgpack message: {0}",
                            token.ToString(Formatting.None)));

                        WampMessage<JToken> message = mMessageFormatter.Parse(token);
                        
                        return message;
                    }
                    catch (Exception ex)
                    {
                        mLogger.ErrorFormat(ex, "Failed parsing msgpack message: {0}",
                            Convert.ToBase64String(raw));

                        throw;
                    }
                }
            }
        }

        public byte[] Format(WampMessage<object> message)
        {
            object[] array = mMessageFormatter.Format(message);

            mLogger.Debug(() => string.Format("Formatting message: {0}",
                JToken.FromObject(array).ToString(Formatting.None)));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (MessagePackWriter writer = new MessagePackWriter(memoryStream))
                {
                    mSerializer.Serialize(writer, array);
                    memoryStream.Position = 0;
                    byte[] result = memoryStream.ToArray();

                    mLogger.Debug(() => string.Format("Formatted message: {0}",
                        Convert.ToBase64String(result)));
                    
                    return result;
                }
            }
        }
    }
}