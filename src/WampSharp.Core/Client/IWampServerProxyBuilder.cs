using WampSharp.Core.Contracts;
using WampSharp.Core.Listener;

namespace WampSharp.Core.Client
{
    public interface IWampServerProxyBuilder<TMessage>
    {
        IWampServer Create(IWampClient<TMessage> client, IWampConnection<TMessage> connection);
    }
}