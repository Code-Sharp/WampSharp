using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.Core.Listener
{
    public interface IWampBinding<TMessage> : IWampBinding
    {
        WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message);
        
        IWampFormatter<TMessage> Formatter { get; }        
    }

    public interface IWampTextBinding<TMessage> : IWampBinding<TMessage>
    {
        WampMessage<TMessage> Parse(string message);
    }

    public interface IWampBinaryBinding<TMessage> : IWampBinding<TMessage>
    {
        WampMessage<TMessage> Parse(byte[] bytes);
    }

    public interface IWampBinding
    {
        string Name { get; }
    }
}