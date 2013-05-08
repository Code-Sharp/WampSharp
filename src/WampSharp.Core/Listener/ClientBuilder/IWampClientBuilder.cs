using WampSharp.Core.Contracts;
using WampSharp.Core.Contracts.V1;

namespace WampSharp.Core.Listener
{
    public interface IWampClientBuilder<TMessage>
    {
        IWampClient Create(IWampConnection<TMessage> connection);
    }
}