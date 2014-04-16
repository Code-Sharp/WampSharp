using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.Client
{
    public class WampChannelFactory<TMessage>
    {
        private readonly IWampBinding<TMessage> mBinding;
        private IWampServerProxyBuilder<TMessage, WampClient<TMessage>, IWampServerProxy> mFactory;

        public WampChannelFactory(IWampBinding<TMessage> binding)
        {
            mBinding = binding;

            IWampFormatter<TMessage> formatter = mBinding.Formatter;

            mFactory =
                new WampServerProxyBuilder<TMessage, WampClient<TMessage>, IWampServerProxy>(
                    new WampOutgoingRequestSerializer<TMessage>(formatter),
                    new WampServerProxyOutgoingMessageHandlerBuilder<TMessage, WampClient<TMessage>>
                        (new WampServerProxyIncomingMessageHandlerBuilder<TMessage, WampClient<TMessage>>(formatter)));
        }


        public WampChannel<TMessage> CreateChannel(string realm, IControlledWampConnection<TMessage> connection)
        {
            var wampRealmProxyFactory = 
                new WampRealmProxyFactory(this, realm, connection);

            WampClient<TMessage> client = 
                new WampClient<TMessage>(wampRealmProxyFactory);
            
            return new WampChannel<TMessage>(connection, client);
        }

        private class WampRealmProxyFactory : IWampRealmProxyFactory<TMessage>
        {
            private readonly WampChannelFactory<TMessage> mParent;
            private readonly string mRealmName;
            private readonly IWampConnection<TMessage> mConnection;

            public WampRealmProxyFactory(WampChannelFactory<TMessage> parent,
                                         string realmName,
                                         IWampConnection<TMessage> connection)
            {
                mParent = parent;
                mRealmName = realmName;
                mConnection = connection;
            }

            public IWampRealmProxy Build(WampClient<TMessage> client)
            {
                IWampServerProxy proxy = mParent.mFactory.Create(client, mConnection);
                
                WampRealmProxy<TMessage> realm = 
                    new WampRealmProxy<TMessage>(mRealmName, proxy, mParent.mBinding);

                return realm;
            }
        }
    }
}