using WampSharp.Core.Message;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.V2.Binding
{
    public interface IWampBinaryBinding<TMessage> : IWampBinding<TMessage>,
        IWampBinaryMessageParser<TMessage>
    {
        WampMessage<TMessage> Parse(byte[] bytes);
    }
}