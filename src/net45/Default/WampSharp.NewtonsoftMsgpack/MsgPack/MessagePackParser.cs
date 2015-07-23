using System;
using System.IO;
using WampSharp.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Msgpack;
using WampSharp.Core.Message;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.Msgpack
{
    public class MessagePackParser : IWampBinaryMessageParser<JToken>,
        IWampStreamingMessageParser<JToken>
    {
        private readonly JsonSerializer mSerializer;
        private readonly ILog mLogger;
        private readonly JsonWampMessageFormatter mMessageFormatter;

        public MessagePackParser(JsonSerializer serializer)
        {
            mSerializer = serializer;
            mMessageFormatter = new JsonWampMessageFormatter();
            mLogger = LogProvider.GetLogger(this.GetType());
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

        public WampMessage<JToken> Parse(Stream stream)
        {
            try
            {
                using (JsonReader reader = new MessagePackReader(stream) {CloseInput = false})
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
            using (MessagePackWriter writer = new MessagePackWriter(stream) {CloseOutput = false})
            {
                writer.Formatting = Formatting.None;
                object[] array = mMessageFormatter.Format(message);
                mSerializer.Serialize(writer, array);
                writer.Flush();
            }
        }
    }
}