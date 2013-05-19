using System;
using WampSharp.Core.Serialization;

namespace WampSharp.Tests.TestHelpers
{
    public class MockRawFormatter : IWampFormatter<MockRaw>
    {
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
            return (TTarget) message.Value;
        }

        public object Deserialize(Type type, MockRaw message)
        {
            if (type == typeof (MockRaw))
            {
                return message;
            }

            return message.Value;
        }

        public MockRaw Serialize(object value)
        {
            return new MockRaw(value);
        }
    }
}