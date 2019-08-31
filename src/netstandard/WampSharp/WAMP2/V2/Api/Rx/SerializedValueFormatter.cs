using System;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;

namespace WampSharp.V2
{
    internal class SerializedValueFormatter : IWampFormatter<ISerializedValue>
    {
        public static readonly IWampFormatter<ISerializedValue> Value = new SerializedValueFormatter();

        private SerializedValueFormatter()
        {
        }

        public bool CanConvert(ISerializedValue argument, Type type)
        {
            return true;
        }

        public TTarget Deserialize<TTarget>(ISerializedValue message)
        {
            return message.Deserialize<TTarget>();
        }

        public object Deserialize(Type type, ISerializedValue message)
        {
            return message.Deserialize(type);
        }

        public ISerializedValue Serialize(object value)
        {
            return new SerializedValue<object>(WampObjectFormatter.Value, value);
        }
    }
}