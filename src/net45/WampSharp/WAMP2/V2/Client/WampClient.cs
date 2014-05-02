using System;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.Client
{
    public class WampClient<TMessage> : IWampSessionClientExtended<TMessage>,
                                        IWampCallee<TMessage>,
                                        IWampCaller<TMessage>,
                                        IWampPublisher<TMessage>,
                                        IWampSubscriber<TMessage>
    {
        private readonly IWampRealmProxy mRealm;
        private readonly IWampSessionClientExtended<TMessage> mSession;

        public WampClient(IWampRealmProxyFactory<TMessage> realmFactory)
        {
            mRealm = realmFactory.Build(this);
            mSession = new SessionClient<TMessage>(this.Realm);
        }

        public void Challenge(string challenge, TMessage extra)
        {
            SessionClient.Challenge(challenge, extra);
        }

        public void Welcome(long session, TMessage details)
        {
            SessionClient.Welcome(session, details);
        }

        public void Abort(TMessage details, string reason)
        {
            SessionClient.Abort(details, reason);
        }

        public void Goodbye(TMessage details, string reason)
        {
            SessionClient.Goodbye(details, reason);
        }

        public void Heartbeat(int incomingSeq, int outgoingSeq)
        {
            SessionClient.Heartbeat(incomingSeq, outgoingSeq);
        }

        public void Heartbeat(int incomingSeq, int outgoingSeq, string discard)
        {
            SessionClient.Heartbeat(incomingSeq, outgoingSeq, discard);
        }

        public long Session
        {
            get { return SessionClient.Session; }
        }

        public IWampRealmProxy Realm
        {
            get
            {
                return mRealm;
            }
        }

        public IWampCallee<TMessage> Callee
        {
            get
            {
                return this.Realm.RpcCatalog as IWampCallee<TMessage>;
            }
        }

        public IWampCaller<TMessage> Caller
        {
            get
            {
                return this.Realm.RpcCatalog as IWampCaller<TMessage>;
            }
        }

        public Task OpenTask
        {
            get
            {
                return SessionClient.OpenTask;
            }
        }

        private IWampSessionClientExtended<TMessage> SessionClient
        {
            get
            {
                return mSession;
            }
        }

        public IWampPublisher<TMessage> Publisher
        {
            get
            {
                return Realm.TopicContainer as IWampPublisher<TMessage>;
            }
        }

        public IWampSubscriber<TMessage> Subscriber
        {
            get
            {
                return Realm.TopicContainer as IWampSubscriber<TMessage>;
            }
        }

        public void OnConnectionOpen()
        {
            SessionClient.OnConnectionOpen();
        }

        public void OnConnectionClosed()
        {
            SessionClient.OnConnectionClosed();
        }

        public void Registered(long requestId, long registrationId)
        {
            Callee.Registered(requestId, registrationId);
        }

        public void Unregistered(long requestId)
        {
            Callee.Unregistered(requestId);
        }

        public void Invocation(long requestId, long registrationId, TMessage details)
        {
            Callee.Invocation(requestId, registrationId, details);
        }

        public void Invocation(long requestId, long registrationId, TMessage details, TMessage[] arguments)
        {
            Callee.Invocation(requestId, registrationId, details, arguments);
        }

        public void Invocation(long requestId, long registrationId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            Callee.Invocation(requestId, registrationId, details, arguments, argumentsKeywords);
        }

        public void Interrupt(long requestId, TMessage options)
        {
            Callee.Interrupt(requestId, options);
        }

        public void Subscribed(long requestId, long subscriptionId)
        {
            Subscriber.Subscribed(requestId, subscriptionId);
        }

        public void Unsubscribed(long requestId, long subscriptionId)
        {
            Subscriber.Unsubscribed(requestId, subscriptionId);
        }

        public void Event(long subscriptionId, long publicationId, TMessage details)
        {
            Subscriber.Event(subscriptionId, publicationId, details);
        }

        public void Event(long subscriptionId, long publicationId, TMessage details, TMessage[] arguments)
        {
            Subscriber.Event(subscriptionId, publicationId, details, arguments);
        }

        public void Event(long subscriptionId, long publicationId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            Subscriber.Event(subscriptionId, publicationId, details, arguments, argumentsKeywords);
        }

        public void Result(long requestId, TMessage details)
        {
            Caller.Result(requestId, details);
        }

        public void Result(long requestId, TMessage details, TMessage[] arguments)
        {
            Caller.Result(requestId, details, arguments);
        }

        public void Result(long requestId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            Caller.Result(requestId, details, arguments, argumentsKeywords);
        }

        public void Published(long requestId, long publicationId)
        {
            Publisher.Published(requestId, publicationId);
        }
    }
}