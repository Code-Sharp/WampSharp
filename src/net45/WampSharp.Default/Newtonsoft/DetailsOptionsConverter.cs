using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Newtonsoft
{
    public class DetailsOptionsConverter : JsonConverter
    {
        private IWampFormatter<JToken> mFormatter;

        public DetailsOptionsConverter(IWampFormatter<JToken> formatter)
        {
            mFormatter = formatter;
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.ReadFrom(reader);

            WampDetailsOptions options = 
                (WampDetailsOptions)Activator.CreateInstance(objectType);

            JsonReader jsonReader = token.CreateReader();
            
            serializer.Populate(jsonReader, options);

            options.OriginalValue = new SerializedValue<JToken>(mFormatter, token);

            return options;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (WampDetailsOptions).IsAssignableFrom(objectType);
        }
    }
}