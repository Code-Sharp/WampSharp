using WampSharp.Core.Message;

namespace WampSharp.Newtonsoft
{
    /// <summary>
    /// Used in order to convert a JSON message to a
    /// <see cref="WampMessage{TMessage}"/> and vice-versa.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    internal interface IWampMessageFormatter<TMessage>
    {
        /// <summary>
        /// Parses a <see cref="WampMessage{TMessage}"/>
        /// from a JSON-array.
        /// </summary>
        /// <param name="message">The given JSON array.</param>
        /// <returns>The parsed <see cref="WampMessage{TMessage}"/>.</returns>
        WampMessage<TMessage> Parse(TMessage message);

        /// <summary>
        /// Converts a <see cref="WampMessage{TMessage}"/>
        /// to a JSON-array.
        /// </summary>
        /// <param name="message">The given <see cref="WampMessage{TMessage}"/>.</param>
        /// <returns>The converted JSON array</returns>
        object[] Format(WampMessage<object> message);
    }
}