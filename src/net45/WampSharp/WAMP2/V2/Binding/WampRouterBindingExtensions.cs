using WampSharp.Core.Listener;
using WampSharp.V2.Realm;
using WampSharp.V2.Realm.Binded;

namespace WampSharp.V2.Binding
{
    internal static class WampRouterBindingExtensions
    {
        public static IWampBindingHost CreateHost<TMessage>
            (this IWampBinding<TMessage> binding,
                IWampHostedRealmContainer realmContainer,
                IWampConnectionListener<TMessage> connectionListener)
        {
            IWampRouterBinding<TMessage> routerBinding = binding as IWampRouterBinding<TMessage>;

            if (routerBinding != null)
            {
                return routerBinding.CreateHost(realmContainer, connectionListener);
            }
            else
            {
                IWampRouterBuilder routerBuilder = new WampRouterBuilder();

                WampBindingHost<TMessage> result =
                    new WampBindingHost<TMessage>(realmContainer,
                                                  routerBuilder,
                                                  connectionListener,
                                                  binding);

                return result;
            }
        }
    }
}