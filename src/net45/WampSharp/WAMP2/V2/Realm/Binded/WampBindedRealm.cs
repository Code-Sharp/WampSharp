using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm.Binded
{
    internal class WampBindedRealm<TMessage> : IWampBindedRealm<TMessage>
    {
        private readonly IWampHostedRealm mRealm;
        private readonly IWampRealmGate mRealmGate;

        public WampBindedRealm(IWampHostedRealm realm,
                               IWampRouterBuilder routerBuilder,
                               IWampSessionServer<TMessage> session,
                               IWampBinding<TMessage> binding,
                               IWampEventSerializer eventSerializer)
        {
            mRealm = realm;
            mRealmGate = realm as IWampRealmGate;

            IWampDealer<TMessage> dealer =
                routerBuilder.CreateDealerHandler(realm, binding);

            IWampBroker<TMessage> broker =
                routerBuilder.CreateBrokerHandler(realm, binding, eventSerializer);

            Server = routerBuilder.CreateServer(session, dealer, broker);
        }

        public IWampServer<TMessage> Server { get; }

        public WelcomeDetails WelcomeDetails
        {
            get
            {
                return new WelcomeDetails()
                {
                    Roles = mRealm.Roles
                };
            }
        }

        public void Hello(long session, HelloDetails helloDetails, WelcomeDetails welcomeDetails)
        {
            mRealmGate.Hello(session, helloDetails, welcomeDetails);
        }

        public void Abort(long session, AbortDetails details, string reason)
        {
            mRealmGate.Abort(session, details, reason);
        }

        public void Goodbye(long session, GoodbyeDetails details, string reason)
        {
            mRealmGate.Goodbye(session, details, reason);
        }

        public void SessionLost(long sessionId)
        {
            mRealmGate.SessionLost(sessionId);
        }

        public string Name
        {
            get
            {
                return mRealm.Name;
            }
        }
    }
}