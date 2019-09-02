using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Newtonsoft
{
    public class DetailsOptionsConverter : JsonConverter
    {
        private readonly IWampFormatter<JToken> mFormatter;

        private ImmutableDictionary<Type, Func<WampDetailsOptions>> mTypeToConstructor =
            ImmutableDictionary<Type, Func<WampDetailsOptions>>.Empty;

        public DetailsOptionsConverter(IWampFormatter<JToken> formatter)
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

            WampDetailsOptions options = CreateInstance(objectType);

            JsonReader jsonReader = token.CreateReader();
            
            serializer.Populate(jsonReader, options);

            options.OriginalValue = new SerializedValue<JToken>(mFormatter, token);

            return options;
        }

        private WampDetailsOptions CreateInstance(Type objectType)
        {
            Func<WampDetailsOptions> constructor = GetConstructor(objectType);

            return constructor();
        }

        private Func<WampDetailsOptions> GetConstructor(Type objectType)
        {

            if (!mTypeToConstructor.TryGetValue(objectType, out Func<WampDetailsOptions> constructor))
            {
                constructor = GenerateConstructor(objectType);

                ImmutableInterlocked.TryAdd(ref mTypeToConstructor, objectType, constructor);
            }

            return constructor;
        }

        private Func<WampDetailsOptions> GenerateConstructor(Type objectType)
        {
            Expression<Func<WampDetailsOptions>> expression =
                Expression.Lambda<Func<WampDetailsOptions>>
                    (Expression.New(objectType));

            return expression.Compile();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (WampDetailsOptions).IsAssignableFrom(objectType);
        }
    }
}