using System;
using WampSharp.Core.Serialization;

namespace WampSharp.V2
{
    /// <summary>
    /// Represents a serialized value.
    /// </summary>
    /// <remarks>
    /// This allows users to work with serialized values without using a
    /// <see cref="IWampFormatter{TMessage}"/>.
    /// </remarks>
    public interface ISerializedValue
    {
        /// <summary>
        /// Deserializes the underlying value to the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The deserialized value.</returns>
        T Deserialize<T>();

        /// <summary>
        /// Deserializes the underlying value to the given type.
        /// </summary>
        /// <param name="type">The deserialized type.</param>
        /// <returns>The deserialized value.</returns>
        object Deserialize(Type type);    
    }
}