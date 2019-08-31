using WampSharp.Core.Message;

namespace WampSharp.V2.Binding.Parsers
{
    /// <summary>
    /// Parses string messages from the stream into <see cref="WampMessage{TMessage}"/>s
    /// and vice versa.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampTextMessageParser<TMessage> : IWampMessageParser<TMessage, string>
    {
    }
}