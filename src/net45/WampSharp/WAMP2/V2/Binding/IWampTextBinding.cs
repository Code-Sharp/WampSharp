using WampSharp.V2.Binding.Parsers;

namespace WampSharp.V2.Binding
{
    public interface IWampTextBinding<TMessage> : IWampBinding<TMessage>,
        IWampTextMessageParser<TMessage>
    {
    }
}