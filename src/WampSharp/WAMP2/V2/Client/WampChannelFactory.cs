using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
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
            WampClient<TMessage> client = 
                new WampClient<TMessage>();
            
            IWampServerProxy proxy = mFactory.Create(client, connection);

            client.Realm = 
                new WampRealmProxy<TMessage>(realm, proxy, mBinding);

            return new WampChannel<TMessage>(connection, client, proxy);
        }
    }
}