﻿using WampSharp.V2.Client;
using WampSharp.V2.Testament.Client;

namespace WampSharp.V2.MetaApi
{
    public static class WampRealmProxyExtensions
    {
        public static WampMetaApiServiceProxy GetMetaApiServiceProxy(this IWampRealmProxy realmProxy)
        {
            return new WampMetaApiServiceProxy(realmProxy);
        }

        public static IWampTestamentServiceProxy GetTestamentServiceProxy(this IWampRealmProxy realmProxy)
        {
            return new WampTestamentServiceProxy(realmProxy, CalleeProxyInterceptor.Default);
        }
    }
}