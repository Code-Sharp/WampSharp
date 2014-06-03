using WampSharp.V2.Binding.Parsers;

namespace WampSharp.V2.Binding
{
    /// <summary>
    /// Represents a text format <see cref="IWampBinding{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampTextBinding<TMessage> : IWampBinding<TMessage>,
        IWampTextMessageParser<TMessage>
    {
    }
}