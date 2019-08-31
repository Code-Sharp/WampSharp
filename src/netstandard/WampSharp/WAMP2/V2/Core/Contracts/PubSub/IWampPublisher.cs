using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampPublisher : IWampPublisher<object>, IWampError<object>
    {
         
    }

    public interface IWampPublisher<TMessage>
    {
        [WampHandler(WampMessageType.v2Published)]
        void Published(long requestId, long publicationId);
    }
}