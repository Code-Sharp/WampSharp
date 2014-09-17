using System;
using WampSharp.Core.Serialization;
using WampSharp.V2;

namespace WampSharp.Tests.Wampv2.Integration
{
    internal class SerializedValue<TMessage> : ISerializedValue
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