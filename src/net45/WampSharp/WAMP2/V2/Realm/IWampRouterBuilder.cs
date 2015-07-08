using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm
{
    public interface IWampRouterBuilder
    {
        IWampSessionServer<TMessage> CreateSessionHandler<TMessage>
            (IWampHostedRealmContainer realmContainer,
             IWampBinding<TMessage> binding,
             IWampEventSerializer eventSerializer);

        IWampBroker<TMessage> CreateBrokerHandler<TMessage>
            (IWampRealm realm,
             IWampBinding<TMessage> binding,
             IWampEventSerializer eventSerializer);

        IWampDealer<TMessage> CreateDealerHandler<TMessage>
            (IWampRealm realm,
             IWampBinding<TMessage> binding);

        IWampServer<TMessage> CreateServer<TMessage>
            (IWampSessionServer<TMessage> session,
             IWampDealer<TMessage> dealer,
             IWampBroker<TMessage> broker);
    }
}