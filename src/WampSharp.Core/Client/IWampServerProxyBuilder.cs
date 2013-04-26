using WampSharp.Core.Contracts;
using WampSharp.Core.Listener;

namespace WampSharp.Core.Client
{
    public interface IWampServerProxyBuilder<TMessage>
    {
        IWampServer Create(IWampConnection<TMessage> connection);
    }
}