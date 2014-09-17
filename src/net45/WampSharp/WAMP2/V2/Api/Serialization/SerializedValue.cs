using System;
using WampSharp.Core.Serialization;

namespace WampSharp.V2
{
    public class SerializedValue<TMessage> : ISerializedValue
    {
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly TMessage mValue;

        public SerializedValue(IWampFormatter<TMessage> formatter, TMessage value)
        {
            mFormatter = formatter;
            mValue = value;
        }

        public T Deserialize<T>()
        {
            return mFormatter.Deserialize<T>(mValue);
        }

        public object Deserialize(Type type)
        {
            return mFormatter.Deserialize(type, mValue);
        }
    }
}