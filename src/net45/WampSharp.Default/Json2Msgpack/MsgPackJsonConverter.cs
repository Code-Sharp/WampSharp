using System;
using MsgPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Json2Msgpack
{
    internal class MsgPackJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            MessagePackObject message = (MessagePackObject) value;

            JToken json = MsgpackToJsonConvert.ToJson(message);

            json.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken json = JToken.ReadFrom(reader);

            MessagePackObject message = 
                MsgpackToJsonConvert.ToMessagePack(json);

            return message;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (MessagePackObject);
        }
    }
}