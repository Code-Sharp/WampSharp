namespace WampSharp.Core.Serialization
{
    /// <summary>
    /// Represents a type capable of creating WAMP message serializers.
    /// </summary>
    /// <example>
    /// A WAMP message serializer is a class that looks implements an interface that 
    /// looks like this:
    /// 
    /// public interface IMySerializer{TMessage}
    /// {
    ///     [WampMessageHandler(WampMessageType.v1Event)]
    ///     WampMessage{TMessage} Event(string topicUri, object @event);
    /// }
    /// 
    /// It is used in order to avoid multiple messages serialization of
    /// message that are sent to multiple clients.
    /// </example>
    public interface IWampMessageSerializerFactory
    {
        /// <summary>
        /// Creates a WAMP message serializer of a given type.
        /// </summary>
        /// <typeparam name="TProxy">The WAMP message serializer given type.</typeparam>
        /// <returns>The created WAMP message serializer.</returns>
        TProxy GetSerializer<TProxy>() where TProxy : class;
    }
}