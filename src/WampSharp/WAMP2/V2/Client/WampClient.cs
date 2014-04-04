using WampSharp.Core.Listener;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.Client
{
    public class WampClient<TMessage> : IWampSessionClient<TMessage>,
                                        IWampCallee<TMessage>,
                                        IWampCaller<TMessage>,
                                        IWampPublisher<TMessage>,
                                        IWampSubscriber<TMessage>
    {
        private readonly IWampConnection<TMessage> mConnection;
        private readonly IWampBinding<TMessage> mBinding;
        private IWampSessionClient<TMessage> mSession;
        private IWampCallee<TMessage> mCallee;
        private IWampCaller<TMessage> mCaller;
        private IWampPublisher<TMessage> mPublisher;
        private IWampSubscriber<TMessage> mSubscriber;
        private IWampRealmProxy mRealm;

        public WampClient(IWampConnection<TMessage> connection, IWampBinding<TMessage> binding)
        {
            mSession = new SessionClient<TMessage>(this);
            mConnection = connection;
            mBinding = binding;
        }

        public IWampRealmProxy Realm
        {
            get
            {
                return mRealm;
            }
            set
            {
                mRealm = value;
            }
        }

        public void Challenge(string challenge, TMessage extra)
        {
            mSession.Challenge(challenge, extra);
        }

        public void Welcome(long session, TMessage details)
        {
            mSession.Welcome(session, details);
        }

        public void Abort(TMessage details, string reason)
        {
            mSession.Abort(details, reason);
        }

        public void Goodbye(TMessage details, string reason)
        {
            mSession.Goodbye(details, reason);
        }

        public void Heartbeat(int incomingSeq, int outgoingSeq)
        {
            mSession.Heartbeat(incomingSeq, outgoingSeq);
        }

        public void Heartbeat(int incomingSeq, int outgoingSeq, string discard)
        {
            mSession.Heartbeat(incomingSeq, outgoingSeq, discard);
        }

        public void Registered(long requestId, long registrationId)
        {
            mCallee.Registered(requestId, registrationId);
        }

        public void Unregistered(long requestId)
        {
            mCallee.Unregistered(requestId);
        }

        public void Invocation(long requestId, long registrationId, TMessage details)
        {
            mCallee.Invocation(requestId, registrationId, details);
        }

        public void Invocation(long requestId, long registrationId, TMessage details, TMessage[] arguments)
        {
            mCallee.Invocation(requestId, registrationId, details, arguments);
        }

        public void Invocation(long requestId, long registrationId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mCallee.Invocation(requestId, registrationId, details, arguments, argumentsKeywords);
        }

        public void Interrupt(long requestId, TMessage options)
        {
            mCallee.Interrupt(requestId, options);
        }

        public void Subscribed(long requestId, long subscriptionId)
        {
            mSubscriber.Subscribed(requestId, subscriptionId);
        }

        public void Unsubscribed(long requestId, long subscriptionId)
        {
            mSubscriber.Unsubscribed(requestId, subscriptionId);
        }

        public void Event(long subscriptionId, long publicationId, TMessage details)
        {
            mSubscriber.Event(subscriptionId, publicationId, details);
        }

        public void Event(long subscriptionId, long publicationId, TMessage details, TMessage[] arguments)
        {
            mSubscriber.Event(subscriptionId, publicationId, details, arguments);
        }

        public void Event(long subscriptionId, long publicationId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mSubscriber.Event(subscriptionId, publicationId, details, arguments, argumentsKeywords);
        }

        public void Result(long requestId, TMessage details)
        {
            mCaller.Result(requestId, details);
        }

        public void Result(long requestId, TMessage details, TMessage[] arguments)
        {
            mCaller.Result(requestId, details, arguments);
        }

        public void Result(long requestId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mCaller.Result(requestId, details, arguments, argumentsKeywords);
        }

        public void Published(long requestId, long publicationId)
        {
            mPublisher.Published(requestId, publicationId);
        }

        internal void Connected()
        {
            mCallee = Realm.RpcCatalog as IWampCallee<TMessage>;
            mCaller = Realm.RpcCatalog as IWampCaller<TMessage>;
        }
    }
}