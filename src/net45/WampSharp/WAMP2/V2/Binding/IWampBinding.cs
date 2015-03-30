using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.Binding
{
    /// <summary>
    /// Represents a WAMP binding.
    /// A WAMP binding is method for serializing messages from/to raw format.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampBinding<TMessage> : IWampBinding
    {
        /// <summary>
        /// Gets a raw message representing the given <see cref="WampMessage{TMessage}"/>.
        /// </summary>
        /// <param name="message">The given <see cref="WampMessage{TMessage}"/>.</param>
        /// <returns>The raw message.</returns>
        /// <remarks>A raw <see cref="WampMessage{TMessage}"/> is a
        /// <see cref="WampMessage{TMessage}"/> with additional properties
        /// which include a raw format of the message. This allows optimization
        /// of serializing the exact same message for multiple client.</remarks>
        WampMessage<object> GetRawMessage(WampMessage<object> message);
        
        /// <summary>
        /// Get the <see cref="IWampFormatter{TMessage}"/> this binding serializes
        /// or deserializes messages with.
        /// </summary>
        IWampFormatter<TMessage> Formatter { get; }
    }

    /// <summary>
    /// A non-generic base class <see cref="IWampBinding{TMessage}"/>.
    /// </summary>
    public interface IWampBinding
    {
        /// <summary>
        /// The name of the current binding.
        /// </summary>
        string Name { get; }
    }
}