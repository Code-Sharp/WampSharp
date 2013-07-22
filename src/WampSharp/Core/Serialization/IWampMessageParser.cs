using WampSharp.Core.Message;

namespace WampSharp.Core.Serialization
{
    /// <summary>
    /// Parses string messages from the stream into <see cref="WampMessage{TMessage}"/>s
    /// and vice versa.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampMessageParser<TMessage>
    {
        /// <summary>
        /// Parses a text message to <see cref="WampMessage{TMessage}"/>.
        /// </summary>
        /// <param name="text">The given text message.</param>
        /// <returns>The converted <see cref="WampMessage{TMessage}"/>.</returns>
        WampMessage<TMessage> Parse(string text);

        /// <summary>
        /// Formats a <see cref="WampMessage{TMessage}"/> to string.
        /// </summary>
        /// <param name="message">The given <see cref="WampMessage{TMessage}"/>.</param>
        /// <returns>A string representing the given <see cref="WampMessage{TMessage}"/>.</returns>
        string Format(WampMessage<TMessage> message);
    }
}