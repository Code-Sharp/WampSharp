using System;

namespace WampSharp.Core.Serialization
{
    public interface IWampFormatter<TMessage>
    {
        bool CanConvert(TMessage argument, Type type);

        TMessage Parse(string message);
        string Format(TMessage message);

        TTarget Deserialize<TTarget>(TMessage message);
        object Deserialize(Type type, TMessage message);

        TMessage Serialize(object value);
    }
}