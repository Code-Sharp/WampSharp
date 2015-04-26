using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Serialization;

namespace WampSharp.Newtonsoft
{
    public class JsonFormatter : IWampFormatter<JToken>
    {
        private readonly JsonSerializer mSerializer;

        public JsonFormatter() : this(new JsonSerializer())
        {
        }

        public JsonFormatter(JsonSerializer serializer)
        {
            mSerializer = serializer;
            // Maybe this will do trouble with null in arguments?
            mSerializer.NullValueHandling = NullValueHandling.Ignore;
            mSerializer.Converters.Add(new DetailsOptionsConverter(this));
            mSerializer.Converters.Add(new SerializedValueConverter(this));
        }

        public bool CanConvert(JToken argument, Type type)
        {
            if (type == typeof (JToken))
            {
                return true;
            }

            switch (argument.Type)
            {
                case JTokenType.Array:
                    {
                        return type.IsArray;
                    }
                case JTokenType.Boolean:
                    {
                        return type == typeof (bool);
                    }
                case JTokenType.Bytes:
                    {
                        return type == typeof (byte[]);
                    }
                case JTokenType.Date:
                    {
                        return type == typeof (DateTime);
                    }
                case JTokenType.Float:
                    {
                        return type == typeof (float) ||
                               type == typeof (double);
                    }
                case JTokenType.Guid:
                    {
                        return type == typeof (Guid);
                    }
                case JTokenType.Integer:
                    {
                        return type == typeof (int);
                    }
                case JTokenType.String:
                    {
                        return type == typeof (string);
                    }
                case JTokenType.TimeSpan:
                    {
                        return type == typeof (TimeSpan);
                    }
                case JTokenType.Object:
                    {
                        return true;
                    }
                default:
                    return false;
            }
        }

        public TTarget Deserialize<TTarget>(JToken message)
        {
            return mSerializer.Deserialize<TTarget>(message.CreateReader());
        }

        public object Deserialize(Type type, JToken message)
        {
            return mSerializer.Deserialize(message.CreateReader(), type);
        }

        public JToken Serialize(object value)
        {
            if (value == null)
            {
                return new JValue((object) null);
            }

            using (JTokenWriter jtokenWriter = new JTokenWriter())
            {
                mSerializer.Serialize(jtokenWriter, value);
                return jtokenWriter.Token;
            }
        }
    }
}