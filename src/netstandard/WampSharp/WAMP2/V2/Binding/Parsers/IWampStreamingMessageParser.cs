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
        WampMessage<TMessage> Parse(Stream stream);

        /// <summary>
        /// Formats a <see cref="WampMessage{TMessage}"/> to a raw format.
        /// </summary>
        /// <param name="message">The given <see cref="WampMessage{TMessage}"/>.</param>
        /// <param name="stream">The stream to write the message to.</param>
        void Format(WampMessage<object> message, Stream stream);
    }
}