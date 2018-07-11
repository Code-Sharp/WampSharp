using System;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    /// <summary>
    /// A class used by the <see cref="IWampHostedRealmContainer"/> of
    /// <see cref="WampHostBase"/>. Actually, the user isn't aware about it.
    /// It mostly adds <see cref="IWampHostedRealm.SessionClosed"/> and
    /// <see cref="IWampHostedRealm.SessionCreated"/> events.
    /// This class is wrapped by <see cref="WampServiceHostedRealm"/> eventually,
    /// so the user never calls <see cref="Services"/> and <see cref="SessionId"/>,
    /// but they shouldn't really be here. But I didn't want to break <see cref="IWampHostedRealm"/>
    /// and I don't have a good name for an interface having only these events. So it stays this way.
    /// </summary>
    internal class WampHostedRealm : IWampHostedRealm, IWampRealmGate
    {
        private readonly IWampRealm mUnderlyingRealm;

        public WampHostedRealm(IWampRealm underlyingRealm)
        {
            mUnderlyingRealm = underlyingRealm;
        }

        public string Name => mUnderlyingRealm.Name;

        public IWampRpcOperationCatalog RpcCatalog => mUnderlyingRealm.RpcCatalog;

        public IWampTopicContainer TopicContainer => mUnderlyingRealm.TopicContainer;

        public IWampRealmServiceProvider Services => throw new NotSupportedException();

        public RouterRoles Roles { get; } = new RouterRoles()
        {
            Dealer = new DealerFeatures()
            {
                PatternBasedRegistration = true,
                SharedRegistration = true,
                CallerIdentification = true,
                ProgressiveCallResults = true,
                CallCanceling = true
            },
            Broker = new BrokerFeatures()
            {
                PublisherIdentification = true,
                PatternBasedSubscription = true,
                PublisherExclusion = true,
                SubscriberBlackwhiteListing = true,
                EventRetention = true
            }
        };

        public long SessionId => throw new NotSupportedException();

        public event EventHandler<WampSessionCreatedEventArgs> SessionCreated;

        public event EventHandler<WampSessionCloseEventArgs> SessionClosed;

        public void Hello(long sessionId, HelloDetails helloDetails, WelcomeDetails welcomeDetails, IWampSessionTerminator terminator)
        {
            RaiseSessionCreated(new WampSessionCreatedEventArgs(sessionId, helloDetails, welcomeDetails, terminator));
        }

        public void Goodbye(long session, GoodbyeDetails details, string reason)
        {
            RaiseSessionClosed(SessionCloseType.Goodbye, session, details, reason);
        }

        public void Abort(long session, AbortDetails details, string reason)
        {
            RaiseSessionClosed(SessionCloseType.Abort, session, details, reason);
        }

        public void SessionLost(long sessionId)
        {
            RaiseSessionClosed(SessionCloseType.Disconnection, sessionId, null, null);
        }

        private void RaiseSessionClosed(SessionCloseType sessionCloseType, long session, GoodbyeAbortDetails details, string reason)
        {
            RaiseSessionClosed
                (new WampSessionCloseEventArgs(sessionCloseType, session,
                                               details, reason));
        }

        protected virtual void RaiseSessionCreated(WampSessionCreatedEventArgs e)
        {
            SessionCreated?.Invoke(this, e);
        }

        protected virtual void RaiseSessionClosed(WampSessionCloseEventArgs e)
        {
            SessionClosed?.Invoke(this, e);
        }
    }
}