using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Newtonsoft.Json.Linq;

namespace WampSharp.LiteDB
{
    internal class JsonToBson
    {
        public static BsonValue ConvertToBson(JToken value)
        {
            return InnerConvertToBson((dynamic) value);
        }

        private static BsonValue InnerConvertToBson(JValue value)
        {
            return new BsonValue(value.Value);
        }

        private static BsonValue InnerConvertToBson(JObject value)
        {
            Dictionary<string, BsonValue> result = new Dictionary<string, BsonValue>();

            foreach (KeyValuePair<string, JToken> propertyNameToValue in value)
            {
                result[propertyNameToValue.Key] = ConvertToBson(propertyNameToValue.Value);
            }

            return new BsonValue(result);
        }

        private static BsonValue InnerConvertToBson(JArray value)
        {
            return new BsonArray(value.Select(x => ConvertToBson(x)));
        }
    }
}
