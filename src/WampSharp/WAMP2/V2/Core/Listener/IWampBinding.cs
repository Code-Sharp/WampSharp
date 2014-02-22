using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.Core.Listener
{
    public interface IWampBinding<TMessage> : IWampBinding
    {
        WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message);
        IWampFormatter<TMessage> Formatter { get; }
    }

    public interface IWampBinding
    {
        string Name { get; }
    }
}