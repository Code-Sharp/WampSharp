using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Message;
using WampSharp.Logging;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.Newtonsoft
{
    public abstract class BinaryJTokenMessageParser : IWampBinaryMessageParser<JToken>
    {
        private readonly JsonSerializer mSerializer;
        private readonly ILog mLogger;
        private readonly JsonWampMessageFormatter mMessageFormatter;

        public BinaryJTokenMessageParser(JsonSerializer serializer)
        {
            mSerializer = serializer;
            mMessageFormatter = new JsonWampMessageFormatter();
            mLogger = LogProvider.GetLogger(this.GetType());
        }

        public WampMessage<JToken> Parse(byte[] raw)
        {
            using (MemoryStream memoryStream = new MemoryStream(raw, false))
            {
                using (JsonReader reader = GetReader(memoryStream))
                {
                    try
                    {
                        if (mLogger.IsDebugEnabled())
                        {
                            mLogger.DebugFormat("Trying to parse binary message: {BinaryMessage}",
                                                Convert.ToBase64String(raw));
                        }

                        JToken token = JToken.Load(reader);

                        if (mLogger.IsDebugEnabled())
                        {
                            mLogger.DebugFormat("Parsed binary message: {Message}",
                                token.ToString(Formatting.None));
                        }

                        WampMessage<JToken> message = mMessageFormatter.Parse(token);
                        
                        return message;
                    }
                    catch (Exception ex)
                    {
                        mLogger.ErrorFormat(ex, "Failed parsing binary message: {BinaryMessage}",
                            Convert.ToBase64String(raw));

                        throw;
                    }
                }
            }
        }

        public byte[] Format(WampMessage<object> message)
        {
            object[] array = mMessageFormatter.Format(message);

            if (mLogger.IsDebugEnabled())
            {
                mLogger.DebugFormat("Formatting message: {Message}",
                    JToken.FromObject(array).ToString(Formatting.None));
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (JsonWriter writer = GetWriter(memoryStream))
                {
                    mSerializer.Serialize(writer, array);
                    memoryStream.Position = 0;
                    byte[] result = memoryStream.ToArray();

                    if (mLogger.IsDebugEnabled())
                    {
                        mLogger.DebugFormat("Formatted message: {BinaryMessage}",
                            Convert.ToBase64String(result));
                    }

                    return result;
                }
            }
        }

        protected abstract JsonReader GetReader(Stream stream);

        protected abstract JsonWriter GetWriter(Stream stream);

        public byte[] GetBytes(byte[] raw)
        {
            return raw;
        }

        public WampMessage<JToken> Parse(Stream stream)
        {
            try
            {
                using (JsonReader reader = GetReader(stream))
                {
                    reader.CloseInput = false;
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
            using (JsonWriter writer = GetWriter(stream))
            {
                writer.CloseOutput = false;
                writer.Formatting = Formatting.None;
                object[] array = mMessageFormatter.Format(message);
                mSerializer.Serialize(writer, array);
                writer.Flush();
            }
        }
    }
}
