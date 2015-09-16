using System;
using System.Collections.Concurrent;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.V2.Client
{
    // TODO: I hate this mechanism, but in order to avoid it, a lot of refactor should
    // TODO: happen in the whole WampServerProxyBuilder mechanism
    // TODO: (mainly receiving the Binding as a parameter to all methods),
    // TODO: and I'm not interested in this right now.
    public class WampChannelFactory : IWampChannelFactory
    {
        private readonly ConcurrentDictionary<IWampBinding, object> mBindingToChannelBuilder =
            new ConcurrentDictionary<IWampBinding, object>();

        public IWampChannel CreateChannel<TMessage>(string realm, IControlledWampConnection<TMessage> connection, IWampBinding<TMessage> binding)
        {
            WampChannelBuilder<TMessage> builder = GetChannelBuilder(binding);
            WampChannel<TMessage> channel = builder.CreateChannel(realm, connection);
            return channel;
        }

        public IWampChannel CreateChannel<TMessage>(string realm, IControlledWampConnection<TMessage> connection,
            IWampBinding<TMessage> binding, IWampClientAuthenticator authenticator)
        {
            WampChannelBuilder<TMessage> builder = GetChannelBuilder(binding);
            WampChannel<TMessage> channel = builder.CreateChannel(realm, connection, authenticator);
            return channel;
        }

        public IWampChannel CreateChannel<TMessage>(string realm,
                                                    Func<IControlledWampConnection<TMessage>> connectionFactory,
                                                    IWampBinding<TMessage> binding)
        {
            return this.CreateChannel(realm, new ReviveClientConnection<TMessage>(connectionFactory), binding);
        }

        public IWampChannel CreateChannel<TMessage>(string realm, Func<IControlledWampConnection<TMessage>> connectionFactory,
                                                    IWampBinding<TMessage> binding,
                                                    IWampClientAuthenticator authenticator)
        {
            return this.CreateChannel(realm,
                                      new ReviveClientConnection<TMessage>(connectionFactory), binding,
                                      authenticator);
        }

        private WampChannelBuilder<TMessage> GetChannelBuilder<TMessage>(IWampBinding<TMessage> binding)
        {
            object result =
                mBindingToChannelBuilder.GetOrAdd
                    (binding,
                     x => new WampChannelBuilder<TMessage>(binding));

            return (WampChannelBuilder<TMessage>) result;
        }
    }
}