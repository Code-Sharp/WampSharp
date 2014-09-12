using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Serialization;

namespace WampSharp.Tests.TestHelpers
{
    public class MockRawFormatter : IWampFormatter<MockRaw>
    {
        private JsonSerializer mSerializer;

        public MockRawFormatter()
        {
            mSerializer = new JsonSerializer();
            mSerializer.Converters.Add(new MockRawConverter());
        }

        public bool CanConvert(MockRaw argument, Type type)
        {
            if (type == typeof (MockRaw))
            {
                return true;
            }
            else
            {
                return type.IsInstanceOfType(argument.Value);
            }
        }

        public TTarget Deserialize<TTarget>(MockRaw message)
        {
            return (TTarget) Deserialize(typeof(TTarget), message);
        }

        public object Deserialize(Type type, MockRaw message)
        {
            if (message.Value == null)
            {
                return null;
            }
            else if (type == typeof (MockRaw))
            {
                return message;
            }
            else if (type.IsInstanceOfType(message.Value))
            {
                return message.Value;                
            }
            else
            {
                JToken token =
                    JToken.FromObject(message.Value, mSerializer);

                return mSerializer.Deserialize(token.CreateReader(), type);
            }
        }

        public MockRaw Serialize(object value)
        {
            return new MockRaw(value);
        }
    }
}