using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    // TODO: Get rid of this?
    internal class WampChannelBuilder<TMessage>
    {
        private readonly IWampBinding<TMessage> mBinding;
        private readonly ManualWampServerProxyBuilder<TMessage> mFactory;

        public WampChannelBuilder(IWampBinding<TMessage> binding)
        {
            mBinding = binding;

            IWampFormatter<TMessage> formatter = mBinding.Formatter;

            mFactory =
                new ManualWampServerProxyBuilder<TMessage>(
                    new WampOutgoingRequestSerializer<TMessage>(formatter),
                    new WampServerProxyOutgoingMessageHandlerBuilder<TMessage, IWampClient<TMessage>>
                        (new WampServerProxyIncomingMessageHandlerBuilder<TMessage, IWampClient<TMessage>>(formatter)));
        }


        public WampChannel<TMessage> CreateChannel(string realm, IControlledWampConnection<TMessage> connection)
        {
            var wampRealmProxyFactory =
                new WampRealmProxyFactory(this, realm, connection);

            WampClient<TMessage> client =
                new WampClient<TMessage>(wampRealmProxyFactory);

            return new WampChannel<TMessage>(connection, client);
        }

        public WampChannel<TMessage> CreateChannel(string realm, IControlledWampConnection<TMessage> connection,
            IWampClientAuthenticator authenticator)
        {
            var wampRealmProxyFactory =
                new WampRealmProxyFactory(this, realm, connection, authenticator);

            WampClient<TMessage> client = 
                new WampClient<TMessage>(wampRealmProxyFactory);
            
            return new WampChannel<TMessage>(connection, client);
        }

        private class WampRealmProxyFactory : IWampRealmProxyFactory<TMessage>
        {
            private readonly WampChannelBuilder<TMessage> mParent;
            private readonly string mRealmName;
            private readonly IWampConnection<TMessage> mConnection;
            private readonly IWampClientAuthenticator mAuthenticator;

            public WampRealmProxyFactory(WampChannelBuilder<TMessage> parent,
                                         string realmName,
                                         IWampConnection<TMessage> connection)
            {
                mParent = parent;
                mRealmName = realmName;
                mConnection = connection;
            }

            public WampRealmProxyFactory(WampChannelBuilder<TMessage> parent,
                                         string realmName,
                                         IWampConnection<TMessage> connection,
                                         IWampClientAuthenticator authenticator)
                : this(parent, realmName, connection)
            {
                mAuthenticator = authenticator;
            }

            public IWampRealmProxy Build(WampClient<TMessage> client)
            {
                IWampServerProxy proxy = mParent.mFactory.Create(client, mConnection);
                
                WampRealmProxy<TMessage> realm =
                    new WampRealmProxy<TMessage>(mRealmName, proxy, mParent.mBinding, mAuthenticator);

                return realm;
            }
        }
    }
}