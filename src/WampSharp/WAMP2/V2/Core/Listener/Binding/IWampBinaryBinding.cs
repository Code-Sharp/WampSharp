using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Listener
{
    public interface IWampBinaryBinding<TMessage> : IWampBinding<TMessage>
    {
        WampMessage<TMessage> Parse(byte[] bytes);
    }
}