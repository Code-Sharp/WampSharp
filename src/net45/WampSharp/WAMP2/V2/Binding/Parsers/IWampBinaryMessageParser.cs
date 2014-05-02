using WampSharp.Core.Message;

namespace WampSharp.V2.Binding.Parsers
{
    /// <summary>
    /// Parses binary messages from the stream into <see cref="WampMessage{TMessage}"/>s
    /// and vice versa.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampBinaryMessageParser<TMessage>
    {
        /// <summary>
        /// Parses a binary message to <see cref="WampMessage{TMessage}"/>.
        /// </summary>
        /// <param name="bytes">The given bytes.</param>
        /// <returns>The converted <see cref="WampMessage{TMessage}"/>.</returns>
        WampMessage<TMessage> Parse(byte[] bytes);

        /// <summary>
        /// Formats a <see cref="WampMessage{TMessage}"/> to a byte array.
        /// </summary>
        /// <param name="message">The given <see cref="WampMessage{TMessage}"/>.</param>
        /// <returns>An array of bytes representing the given <see cref="WampMessage{TMessage}"/>.</returns>
        byte[] Format(WampMessage<TMessage> message);
    }
}