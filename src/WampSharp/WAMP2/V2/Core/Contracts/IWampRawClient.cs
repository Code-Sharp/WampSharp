using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampRawClient<TMessage>
    {
        [WampRawHandler]
        void Message(WampMessage<TMessage> message);
    }
}