using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.V2.Core.Listener
{
    public interface IWampTextBinding<TMessage> : IWampBinding<TMessage>,
        IWampTextMessageParser<TMessage>
    {
    }
}