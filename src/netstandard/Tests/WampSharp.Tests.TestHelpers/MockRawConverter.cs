using System;
using Newtonsoft.Json;

namespace WampSharp.Tests.TestHelpers
{
    public class MockRawConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            MockRaw converted = value as MockRaw;

            serializer.Serialize(writer, converted.Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object deserialize = serializer.Deserialize(reader);

            return new MockRaw(deserialize);
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof (MockRaw));
        }
    }
}