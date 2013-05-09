using WampSharp.Core.Message;

namespace WampSharp.Core.Dispatch
{
    public interface IWampIncomingMessageHandler<TMessage, TClient>
    {
        void HandleMessage(TClient client, WampMessage<TMessage> message);
    }

    public interface IWampIncomingMessageHandler<TMessage>
    {
        void HandleMessage(WampMessage<TMessage> message);         
    }
}