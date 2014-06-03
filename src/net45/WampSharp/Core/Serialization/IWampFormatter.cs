using System;

namespace WampSharp.Core.Serialization
{
    /// <summary>
    /// Represents a messages formatter
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampFormatter<TMessage>
    {
        /// <summary>
        /// Returns a value indicating whether the given argument
        /// can be deserialized to the given type.
        /// </summary>
        /// <param name="argument">The given argument.</param>
        /// <param name="type">The given type.</param>
        /// <returns>A value indicating whether the given argument
        /// can be casted to the given type.</returns>
        [Obsolete("Used only for WAMP1. You don't need to implement this for WAMP2 usages.")]
        bool CanConvert(TMessage argument, Type type);

        /// <summary>
        /// Deserializes the given messages to the given type.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="message">The given message.</param>
        /// <returns>The deserialized message.</returns>
        TTarget Deserialize<TTarget>(TMessage message);

        /// <summary>
        /// Deserializes the given messages to the given type.
        /// </summary>
        /// <param name="type">The given type.</param>
        /// <param name="message">The given message.</param>
        /// <returns>The deserialized message.</returns>
        object Deserialize(Type type, TMessage message);

        /// <summary>
        /// Serializes the given object to the message type.
        /// </summary>
        /// <param name="value">The given object.</param>
        /// <returns>The serialized message.</returns>
        TMessage Serialize(object value);
    }
}