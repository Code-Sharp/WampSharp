using WampSharp.Core.Message;

namespace WampSharp.V2.Binding.Parsers
{
    /// <summary>
    /// Parses raw formatted messages from the stream into <see cref="WampMessage{TMessage}"/>s
    /// and vice versa.
    /// </summary>
    public interface IWampMessageParser<TMessage, TRaw> : IWampStreamingMessageParser<TMessage>
    {
        /// <summary>
        /// Parses a raw message to <see cref="WampMessage{TMessage}"/>.
        /// </summary>
        /// <param name="raw">The given raw messsage.</param>
        /// <returns>The converted <see cref="WampMessage{TMessage}"/>.</returns>
        WampMessage<TMessage> Parse(TRaw raw);

        /// <summary>
        /// Formats a <see cref="WampMessage{TMessage}"/> to a raw format.
        /// </summary>
        /// <param name="message">The given <see cref="WampMessage{TMessage}"/>.</param>
        /// <returns>A raw format representing the given <see cref="WampMessage{TMessage}"/>.</returns>
        TRaw Format(WampMessage<object> message);

        /// <summary>
        /// Serializes a raw message to bytes.
        /// </summary>
        /// <param name="raw">The given raw message.</param>
        /// <returns>Bytes representing the binary format of the given raw message.</returns>
        byte[] GetBytes(TRaw raw);
    }
}