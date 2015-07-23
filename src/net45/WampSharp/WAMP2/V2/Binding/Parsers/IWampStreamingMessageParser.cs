using System.IO;
using WampSharp.Core.Message;

namespace WampSharp.V2.Binding.Parsers
{
    public interface IWampStreamingMessageParser<TMessage>
    {
        /// <summary>
        /// Parses a raw message to <see cref="WampMessage{TMessage}"/>.
        /// </summary>
        /// <returns>The converted <see cref="WampMessage{TMessage}"/>.</returns>
        WampMessage<TMessage> Parse(byte[] bytes, int position, int length);

        /// <summary>
        /// Formats a <see cref="WampMessage{TMessage}"/> to a raw format.
        /// </summary>
        /// <param name="message">The given <see cref="WampMessage{TMessage}"/>.</param>
        /// <param name="bytes">The array to write the message to.</param>
        /// <param name="position">The position in the array to write the message to.</param>
        /// <returns>A raw format representing the given <see cref="WampMessage{TMessage}"/>.</returns>
        int Format(WampMessage<object> message, byte[] bytes, int position);
    }
}