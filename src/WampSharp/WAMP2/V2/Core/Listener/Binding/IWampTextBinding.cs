using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Listener
{
    public interface IWampTextBinding<TMessage> : IWampBinding<TMessage>
    {
        WampMessage<TMessage> Parse(string message);
    }
}