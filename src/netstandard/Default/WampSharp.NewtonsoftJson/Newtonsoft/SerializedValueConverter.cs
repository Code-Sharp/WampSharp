using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2;

namespace WampSharp.Newtonsoft
{
    public class SerializedValueConverter : JsonConverter
    {
        private readonly IWampFormatter<JToken> mFormatter;

        public SerializedValueConverter(IWampFormatter<JToken> formatter)
        {
            mFormatter = formatter;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.ReadFrom(reader);

            SerializedValue<JToken> result = new SerializedValue<JToken>(mFormatter, token);
            
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (ISerializedValue) == objectType;
        }
    }
}