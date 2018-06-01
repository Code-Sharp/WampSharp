using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class WampRawTopic<TMessage> : IWampRawTopic<TMessage>, IWampRawTopicRouterSubscriber, IDisposable
    {
        #region Data Members

        private readonly RawTopicSubscriberBook mSubscriberBook;
        private readonly IWampBinding<TMessage> mBinding; 
        private readonly IWampEventSerializer mSerializer;
        private readonly SubscribeOptions mSubscribeOptions;

        #endregion

        #region Constructor

        public WampRawTopic(string topicUri, SubscribeOptions subscribeOptions, IWampCustomizedSubscriptionId customizedSubscriptionId, IWampEventSerializer serializer, IWampBinding<TMessage> binding)
        {
            mSerializer = serializer;
            mSubscriberBook = new RawTopicSubscriberBook(this);
            TopicUri = topicUri;
            mBinding = binding;
            mSubscribeOptions = subscribeOptions;
            CustomizedSubscriptionId = customizedSubscriptionId;
        }

        #endregion

        #region IRawWampTopic<TMessage> Members

        public void Event<TRaw>(IWampFormatter<TRaw> formatter, long publicationId, PublishOptions options)
        {
            Func<EventDetails, WampMessage<object>> action =
                eventDetails => mSerializer.Event(SubscriptionId, publicationId, eventDetails);

            InnerEvent(options, action);
        }

        public void Event<TRaw>(IWampFormatter<TRaw> formatter, long publicationId, PublishOptions options,
                                TRaw[] arguments)
        {
            Func<EventDetails, WampMessage<object>> action =
                details => mSerializer.Event(SubscriptionId,
                                             publicationId,
                                             details,
                                             arguments.Cast<object>().ToArray());

            InnerEvent(options, action);
        }

        public void Event<TRaw>(IWampFormatter<TRaw> formatter, long publicationId, PublishOptions options, TRaw[] arguments, IDictionary<string, TRaw> argumentsKeywords)
        {
            Func<EventDetails, WampMessage<object>> action =
                details => mSerializer.Event(SubscriptionId, publicationId, details,
                                             arguments.Cast<object>().ToArray(),
                                             argumentsKeywords.ToDictionary(x => x.Key,
                                                                            x => (object) x.Value));

            InnerEvent(options, action);
        }

        private void Publish(WampMessage<object> message, PublishOptions options)
        {
            WampMessage<object> raw = mBinding.GetRawMessage(message);

            IEnumerable<RemoteObserver> subscribers = 
                mSubscriberBook.GetRelevantSubscribers(options);

            foreach (RemoteObserver subscriber in subscribers)
            {
                subscriber.Message(raw);
            }
        }

        private void InnerEvent(PublishOptions options, Func<EventDetails, WampMessage<object>> action)
        {
            EventDetails details = options.GetEventDetails(mSubscribeOptions.Match);

            WampMessage<object> message = action(details);

            Publish(message, options);
        }

        public bool HasSubscribers => mSubscriberBook.HasSubscribers;

        public long SubscriptionId
        {
            get; 
            set;
        }

        public string TopicUri { get; }

        public IDisposable SubscriptionDisposable
        {
            get; 
            set;
        }

        public IWampCustomizedSubscriptionId CustomizedSubscriptionId { get; }

        public void Subscribe(ISubscribeRequest<TMessage> request, SubscribeOptions options)
        {
            RemoteWampTopicSubscriber remoteSubscriber =
                new RemoteWampTopicSubscriber(this.SubscriptionId,
                                              request.Client as IWampSubscriber);

            IWampClientProxy<TMessage> client = request.Client;

            RemoteObserver observer = mSubscriberBook.Subscribe(client);

            if (!observer.IsOpen)
            {
                this.RaiseSubscriptionAdding(remoteSubscriber, options);                
            }

            request.Subscribed(this.SubscriptionId);

            if (!observer.IsOpen)
            {
                this.RaiseSubscriptionAdded(remoteSubscriber, options);

                observer.Open();
            }
        }

        public void Unsubscribe(IUnsubscribeRequest<TMessage> request)
        {
            IWampClientProxy<TMessage> client = request.Client;

            if (mSubscriberBook.Unsubscribe(client))
            {
                this.RaiseSubscriptionRemoving(client.Session);

                request.Unsubscribed();

                this.RaiseSubscriptionRemoved(client.Session);

                if (!this.HasSubscribers)
                {
                    this.RaiseTopicEmpty();
                }
            }
        }

        public void Dispose()
        {
            if (SubscriptionDisposable != null)
            {
                SubscriptionDisposable.Dispose();
                SubscriptionDisposable = null;
            }
        }

        #endregion

        #region ISubscriptionNotifier

        public event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdding;
        public event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdded;
        public event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoving;
        public event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoved;
        public event EventHandler TopicEmpty;

        protected virtual void RaiseSubscriptionAdding(RemoteWampTopicSubscriber subscriber, SubscribeOptions options)
        {
            EventHandler<WampSubscriptionAddEventArgs> handler = SubscriptionAdding;

            if (handler != null)
            {
                WampSubscriptionAddEventArgs args = GetAddEventArgs(subscriber, options);

                handler(this, args);
            }
        }

        protected virtual void RaiseSubscriptionAdded(RemoteWampTopicSubscriber subscriber, SubscribeOptions options)
        {
            EventHandler<WampSubscriptionAddEventArgs> handler = SubscriptionAdded;

            if (handler != null)
            {
                WampSubscriptionAddEventArgs args = GetAddEventArgs(subscriber, options);

                handler(this, args);
            }
        }

        protected virtual void RaiseSubscriptionRemoving(long sessionId)
        {
            EventHandler<WampSubscriptionRemoveEventArgs> handler = SubscriptionRemoving;

            if (handler != null)
            {
                WampSubscriptionRemoveEventArgs args = GetRemoveEventArgs(sessionId);
                handler(this, args);
            }
        }

        protected virtual void RaiseSubscriptionRemoved(long sessionId)
        {
            EventHandler<WampSubscriptionRemoveEventArgs> handler = SubscriptionRemoved;

            if (handler != null)
            {
                WampSubscriptionRemoveEventArgs args = GetRemoveEventArgs(sessionId);
                handler(this, args);
            }
        }

        protected virtual void RaiseTopicEmpty()
        {
            TopicEmpty?.Invoke(this, EventArgs.Empty);
        }

        private WampSubscriptionAddEventArgs GetAddEventArgs(RemoteWampTopicSubscriber subscriber, SubscribeOptions options)
        {
            return new WampSubscriptionAddEventArgs(subscriber, options);
        }

        private WampSubscriptionRemoveEventArgs GetRemoveEventArgs(long sessionId)
        {
            return new WampSubscriptionRemoveEventArgs(sessionId, this.SubscriptionId);
        }

        #endregion

        #region Nested Types

        private class Subscription
        {
            private readonly WampRawTopic<TMessage> mParent;
            private readonly IWampClientProxy<TMessage> mClient;

            public Subscription(WampRawTopic<TMessage> parent, IWampClientProxy<TMessage> client, RemoteObserver observer)
            {
                mParent = parent;
                mClient = client;
                Observer = observer;
            }

            public RemoteObserver Observer { get; }

            public void Open()
            {
                IWampConnectionMonitor monitor = mClient as IWampConnectionMonitor;
                monitor.ConnectionClosed += OnConnectionClosed;                
            }

            private void OnConnectionClosed(object sender, EventArgs e)
            {
                mParent.Unsubscribe(new DisconnectUnsubscribeRequest(mClient));
                IWampConnectionMonitor monitor = sender as IWampConnectionMonitor;
                monitor.ConnectionClosed -= OnConnectionClosed;
            }

            protected bool Equals(Subscription other)
            {
                return Equals(mClient.Session, other.mClient.Session);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Subscription) obj);
            }

            public override int GetHashCode()
            {
                return mClient.Session.GetHashCode();
            }

            private class DisconnectUnsubscribeRequest : IUnsubscribeRequest<TMessage>
            {
                public DisconnectUnsubscribeRequest(IWampClientProxy<TMessage> client)
                {
                    Client = client;
                }

                public IWampClientProxy<TMessage> Client { get; }

                public void Unsubscribed()
                {
                }
            }
        }

        private class RemoteObserver : IWampRawClient
        {
            private readonly IWampRawClient mClient;

            public RemoteObserver(IWampRawClient client)
            {
                mClient = client;
                IWampClientProxy casted = mClient as IWampClientProxy;
                SessionId = casted.Session;
            }

            public RemoteObserver(long sessionId)
            {
                SessionId = sessionId;
            }

            public long SessionId { get; }

            public bool IsOpen { get; private set; } = false;

            public void Open()
            {
                IsOpen = true;
            }

            public void Message(WampMessage<object> message)
            {
                if (IsOpen)
                {
                    mClient.Message(message);
                }
            }

            protected bool Equals(RemoteObserver other)
            {
                return (ReferenceEquals(mClient, other.mClient)) ||
                       SessionId == other.SessionId;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) {return false;}
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((RemoteObserver) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return SessionId.GetHashCode();
                }
            }
        }

        private class RawTopicSubscriberBook
        {
            private ImmutableDictionary<long, Subscription> mSessionIdToSubscription =
                ImmutableDictionary<long, Subscription>.Empty;

            private ImmutableDictionary<string, ImmutableList<Subscription>> mAuthenticationIdToSubscription =
                ImmutableDictionary<string, ImmutableList<Subscription>>.Empty;

            private ImmutableDictionary<string, ImmutableList<Subscription>> mAuthenticationRoleToSubscription =
                ImmutableDictionary<string, ImmutableList<Subscription>>.Empty;

            private readonly object mLock = new object();

            private ImmutableHashSet<RemoteObserver> mRemoteObservers = ImmutableHashSet<RemoteObserver>.Empty;

            private readonly WampRawTopic<TMessage> mRawTopic;
            
            public RawTopicSubscriberBook(WampRawTopic<TMessage> rawTopic)
            {
                mRawTopic = rawTopic;
            }

            public bool HasSubscribers => mRemoteObservers.Count > 0;

            public RemoteObserver Subscribe(IWampClientProxy<TMessage> client)
            {

                if (!mSessionIdToSubscription.TryGetValue(client.Session, out Subscription subscription))
                {
                    RemoteObserver result = new RemoteObserver(client);

                    ImmutableHashSetInterlocked.Add(ref mRemoteObservers, result);

                    subscription = new Subscription(mRawTopic, client, result);

                    ImmutableInterlocked.TryAdd(ref mSessionIdToSubscription, client.Session, subscription);

                    AddAuthenticationData(client, subscription);

                    subscription.Open();
                }

                return subscription.Observer;
            }

            public bool Unsubscribe(IWampClientProxy<TMessage> client)
            {
                bool result;
                ImmutableHashSetInterlocked.Remove(ref mRemoteObservers , new RemoteObserver(client));
                result = ImmutableInterlocked.TryRemove(ref mSessionIdToSubscription, client.Session, out Subscription subscription);
                RemoveAuthenticationData(client);
                return result;
            }

            public IEnumerable<RemoteObserver> GetRelevantSubscribers(PublishOptions options)
            {
                ImmutableHashSet<RemoteObserver> result = mRemoteObservers;

                result = GetEligibleObservers(result, options);

                bool excludeMe = options.ExcludeMe ?? true;
                
                PublishOptionsExtended casted = options as PublishOptionsExtended;

                if (excludeMe && casted != null)
                {
                    result = result.Remove(new RemoteObserver(casted.PublisherId));
                }

                result = RemoveExcludedObservers(result, options);

                return result;
            }

            private ImmutableHashSet<RemoteObserver> GetEligibleObservers(ImmutableHashSet<RemoteObserver> allObservers, PublishOptions options)
            {
                ImmutableHashSet<RemoteObserver> result = allObservers;

                if (options.Eligible != null)
                {
                    var eligibleObservers =
                        GetRemoteObservers(options.Eligible)
                            .ToArray();

                    result = ImmutableHashSet.Create(eligibleObservers);
                }

                if (options.EligibleAuthenticationIds != null)
                {
                    ImmutableHashSet<RemoteObserver> gatheredAuthIdObservers =
                        GatherObservers(mAuthenticationIdToSubscription,
                                        options.EligibleAuthenticationIds);

                    result = result.Intersect(gatheredAuthIdObservers);
                }

                if (options.EligibleAuthenticationRoles != null)
                {
                    ImmutableHashSet<RemoteObserver> gatheredAuthIdObservers =
                        GatherObservers(mAuthenticationRoleToSubscription,
                                        options.EligibleAuthenticationRoles);

                    result = result.Intersect(gatheredAuthIdObservers);
                }

                return result;
            }

            private ImmutableHashSet<RemoteObserver> RemoveExcludedObservers(ImmutableHashSet<RemoteObserver> observers, PublishOptions options)
            {
                ImmutableHashSet<RemoteObserver> result = observers;

                if (options.Exclude != null)
                {
                    var excludedObservers =
                        options.Exclude.Select
                            (sessionId => new RemoteObserver(sessionId));

                    result = result.Except(excludedObservers);
                }

                if (options.ExcludeAuthenticationIds != null)
                {
                    ImmutableHashSet<RemoteObserver> excludedAuthenticationIds =
                        GatherObservers(mAuthenticationIdToSubscription,
                                        options.ExcludeAuthenticationIds);

                    result = result.Except(excludedAuthenticationIds);
                }

                if (options.ExcludeAuthenticationRoles != null)
                {
                    ImmutableHashSet<RemoteObserver> excludedAuthenticationRoles =
                        GatherObservers(mAuthenticationRoleToSubscription,
                                        options.ExcludeAuthenticationRoles);

                    result = result.Except(excludedAuthenticationRoles);
                }

                return result;
            }

            private static ImmutableHashSet<RemoteObserver> GatherObservers
            (IDictionary<string, ImmutableList<Subscription>> dictionary,
             string[] ids)
            {
                ImmutableHashSet<RemoteObserver> result = null;

                if (ids != null)
                {
                    result = ImmutableHashSet<RemoteObserver>.Empty;

                    foreach (string id in ids)
                    {

                        if (dictionary.TryGetValue(id, out ImmutableList<Subscription> subscriptions))
                        {
                            result = result.Union(subscriptions.Select(x => x.Observer));
                        }
                    }
                }

                return result;
            }

            private RemoteObserver GetRemoteObserverById(long sessionId)
            {

                if (mSessionIdToSubscription.TryGetValue(sessionId, out Subscription subscription))
                {
                    return subscription.Observer;
                }

                return null;
            }

            private IEnumerable<RemoteObserver> GetRemoteObservers(long[] sessionIds)
            {
                return sessionIds.Select(id => GetRemoteObserverById(id))
                                 .Where(x => x != null);
            }

            private void MapIdToSubscription(ref ImmutableDictionary<string, ImmutableList<Subscription>> dictionary,
                                             string id,
                                             Subscription subscription)
            {
                lock (mLock)
                {
                    ImmutableList<Subscription> subscriptions =
                        ImmutableInterlocked.GetOrAdd(ref dictionary,
                                                      id,
                                                      x => ImmutableList<Subscription>.Empty);

                    dictionary =
                        dictionary.SetItem(id, subscriptions.Add(subscription));
                }
            }

            private void AddAuthenticationData(IWampClientProxy<TMessage> client, Subscription subscription)
            {
                WelcomeDetails welcomeDetails = client.Authenticator?.WelcomeDetails;

                if (welcomeDetails != null)
                {
                    string authenticationId = welcomeDetails.AuthenticationId;
                    string authenticationRole = welcomeDetails.AuthenticationRole;

                    if (authenticationId != null)
                    {
                        MapIdToSubscription(ref mAuthenticationIdToSubscription,
                                            authenticationId,
                                            subscription);
                    }

                    if (authenticationRole != null)
                    {
                        MapIdToSubscription(ref mAuthenticationRoleToSubscription,
                                            authenticationRole,
                                            subscription);
                    }
                }
            }

            private void RemoveAuthenticationData(IWampClientProxy<TMessage> client)
            {
                WelcomeDetails welcomeDetails = client.Authenticator?.WelcomeDetails;

                if (welcomeDetails != null)
                {
                    string authenticationId = welcomeDetails.AuthenticationId;
                    string authenticationRole = welcomeDetails.AuthenticationRole;
                    Subscription subscription = new Subscription(mRawTopic, client, null);

                    if (authenticationId != null)
                    {
                        RemoveIdToSubscription(ref mAuthenticationIdToSubscription,
                                               authenticationId,
                                               subscription);
                    }

                    if (authenticationRole != null)
                    {
                        RemoveIdToSubscription(ref mAuthenticationRoleToSubscription,
                                               authenticationRole,
                                               subscription);
                    }
                }
            }

            private void RemoveIdToSubscription(ref ImmutableDictionary<string, ImmutableList<Subscription>> dictionary,
                                                string id,
                                                Subscription subscription)
            {
                lock (mLock)
                {

                    if (dictionary.TryGetValue(id, out ImmutableList<Subscription> subscriptions))
                    {
                        subscriptions = subscriptions.Remove(subscription);

                        if (subscriptions.Count != 0)
                        {
                            dictionary =
                                dictionary.SetItem(id, subscriptions);
                        }
                        else
                        {
                            dictionary =
                                dictionary.Remove(id);
                        }
                    }
                }
            }
        }

        #endregion
    }
}