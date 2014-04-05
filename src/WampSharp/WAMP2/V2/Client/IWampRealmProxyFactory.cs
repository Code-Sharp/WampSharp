using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    public interface IWampRealmProxyFactory<TMessage>
    {
        IWampRealmProxy Build(WampClient<TMessage> client);
    }
}