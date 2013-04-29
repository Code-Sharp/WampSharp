using WampSharp.Core.Contracts;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Core.Dispatch
{
    public interface IWampIncomingMessageHandlerBuilder<TMessage>
    {
        IWampIncomingMessageHandler<TMessage> Build(IWampClient<TMessage> client, IWampConnection<TMessage> connection);
    }

    public interface IWampIncomingMessageHandler<TMessage>
    {
        void HandleMessage(IWampClient client, WampMessage<TMessage> message);
    }
}