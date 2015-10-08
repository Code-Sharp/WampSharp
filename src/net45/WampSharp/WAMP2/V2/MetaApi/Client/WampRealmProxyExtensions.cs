using WampSharp.V2.Client;

namespace WampSharp.V2.MetaApi
{
    public static class WampRealmProxyExtensions
    {
        public static WampMetaApiServiceProxy GetMetaApiServiceProxy(this IWampRealmProxy realmProxy)
        {
            return new WampMetaApiServiceProxy(realmProxy);
        }
    }
}