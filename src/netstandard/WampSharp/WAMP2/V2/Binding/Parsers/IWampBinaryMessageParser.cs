using WampSharp.Core.Message;

namespace WampSharp.V2.Binding.Parsers
{
    /// <summary>
    /// Parses binary messages from the stream into <see cref="WampMessage{TMessage}"/>s
    /// and vice versa.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampBinaryMessageParser<TMessage> : IWampMessageParser<TMessage, byte[]>
    {
    }
}