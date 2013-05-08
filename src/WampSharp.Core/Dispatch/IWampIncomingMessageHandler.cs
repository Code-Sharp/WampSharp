using WampSharp.Core.Contracts;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Message;

namespace WampSharp.Core.Dispatch
{
    public interface IWampIncomingMessageHandler<TMessage>
    {
        void HandleMessage(IWampClient client, WampMessage<TMessage> message);
    }
}