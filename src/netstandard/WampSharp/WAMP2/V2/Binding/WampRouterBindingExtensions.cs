using WampSharp.Core.Listener;
using WampSharp.V2.Core;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Binding
{
    internal static class WampRouterBindingExtensions
    {
        public static IWampBindingHost CreateHost<TMessage>
            (this IWampBinding<TMessage> binding,
             IWampHostedRealmContainer realmContainer,
             IWampConnectionListener<TMessage> connectionListener,
             IWampUriValidator uriValidator,
             IWampSessionMapper sessionIdMap)
        {

            if (binding is IWampRouterBinding<TMessage> routerBinding)
            {
                return routerBinding.CreateHost(realmContainer, connectionListener, sessionIdMap);
            }
            else
            {
                IWampRouterBuilder routerBuilder = new WampRouterBuilder(uriValidator);

                WampBindingHost<TMessage> result =
                    new WampBindingHost<TMessage>(realmContainer,
                                                  routerBuilder,
                                                  connectionListener,
                                                  binding,
                                                  sessionIdMap);

                return result;
            }
        }
    }
}