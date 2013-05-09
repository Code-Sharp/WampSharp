using WampSharp.Core.Listener;

namespace WampSharp.Core.Client
{
    public interface IWampServerProxyBuilder<TMessage, TRawClient, TServer>
    {
        TServer Create(TRawClient client, IWampConnection<TMessage> connection);
    }
}