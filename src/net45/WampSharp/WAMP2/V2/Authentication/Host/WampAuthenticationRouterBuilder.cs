using WampSharp.V2.Binding;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Authentication
{
    internal class WampAuthenticationRouterBuilder : WampRouterBuilder
    {
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;

        public WampAuthenticationRouterBuilder(IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
                                               IWampUriValidator uriValidator) :
                                                   base(uriValidator)
        {
            mSessionAuthenticationFactory = sessionAuthenticationFactory;
        }

        public override IWampSessionServer<TMessage> CreateSessionHandler<TMessage>
            (IWampHostedRealmContainer realmContainer,
             IWampBinding<TMessage> binding,
             IWampEventSerializer eventSerializer)
        {
            return new WampAuthenticationSessionServer<TMessage>
                (binding,
                 realmContainer,
                 this,
                 eventSerializer,
                 mSessionAuthenticationFactory);
        }

        public override IWampServer<TMessage> CreateServer<TMessage>(IWampSessionServer<TMessage> session, IWampDealer<TMessage> dealer, IWampBroker<TMessage> broker)
        {
            return new WampAuthenticationServer<TMessage>(session, dealer, broker);
        }
    }
}