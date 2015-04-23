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
        private readonly string mTopicUri;
        private readonly SubscribeOptions mSubscribeOptions;
        private readonly IWampCustomizedSubscriptionId mCustomizedSubscriptionId;

        #endregion

        #region Constructor

        public WampRawTopic(string topicUri, SubscribeOptions subscribeOptions, IWampCustomizedSubscriptionId customizedSubscriptionId, IWampEventSerializer serializer, IWampBinding<TMessage> binding)
        {
            mSerializer = serializer;
            mSubscriberBook = new RawTopicSubscriberBook(this);
            mTopicUri = topicUri;
            mBinding = binding;
            mSubscribeOptions = subscribeOptions;
            mCustomizedSubscriptionId = customizedSubscriptionId;
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

        private EventDetails GetDetails(PublishOptions options)
        {
            EventDetails result = new EventDetails();

            PublishOptionsExtended extendedOptions = 
                options as PublishOptionsExtended;

            bool disclosePublisher = options.DiscloseMe ?? false;

            if (extendedOptions != null)
            {
                if (disclosePublisher)
                {
                    result.Publisher = extendedOptions.PublisherId;
                }

                string match = mSubscribeOptions.Match;

                if (match != "exact")
                {
                    result.Topic = extendedOptions.TopicUri;
                }
            }

            return result;
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
            EventDetails details = GetDetails(options);

            WampMessage<object> message = action(details);

            Publish(message, options);
        }

        public bool HasSubscribers
        {
            get
            {
                return mSubscriberBook.HasSubscribers;
            }
        }

        public long SubscriptionId
        {
            get; 
            set;
        }

        public string TopicUri
        {
            get
            {
                return mTopicUri;
            }
        }

        public IDisposable SubscriptionDisposable
        {
            get; 
            set;
        }

        public IWampCustomizedSubscriptionId CustomizedSubscriptionId
        {
            get { return mCustomizedSubscriptionId; }
        }

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
                observer.Open();

                this.RaiseSubscriptionAdded(remoteSubscriber, options);                
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
            SubscriptionDisposable.Dispose();
            SubscriptionDisposable = null;
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
            EventHandler handler = TopicEmpty;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private WampSubscriptionAddEventArgs GetAddEventArgs(RemoteWampTopicSubscriber subscriber, SubscribeOptions options)
        {
            return new WampSubscriptionAddEventArgs(subscriber, options);
        }

        private static WampSubscriptionRemoveEventArgs GetRemoveEventArgs(long sessionId)
        {
            return new WampSubscriptionRemoveEventArgs(sessionId);
        }

        #endregion

        #region Nested Types

        private class Subscription
        {
            private readonly WampRawTopic<TMessage> mParent;
            private readonly IWampClientProxy<TMessage> mClient;
            private readonly RemoteObserver mObserver;

            public Subscription(WampRawTopic<TMessage> parent, IWampClientProxy<TMessage> client, RemoteObserver observer)
            {
                mParent = parent;
                mClient = client;
                mObserver = observer;

                IWampConnectionMonitor monitor = mClient as IWampConnectionMonitor;
                monitor.ConnectionClosed += OnConnectionClosed;
            }

            public RemoteObserver Observer
            {
                get
                {
                    return mObserver;
                }
            }

            private void OnConnectionClosed(object sender, EventArgs e)
            {
                mParent.Unsubscribe(new DisconnectUnsubscribeRequest(mClient));
                IWampConnectionMonitor monitor = sender as IWampConnectionMonitor;
                monitor.ConnectionClosed -= OnConnectionClosed;
            }

            private class DisconnectUnsubscribeRequest : IUnsubscribeRequest<TMessage>
            {
                private readonly IWampClientProxy<TMessage> mClient;

                public DisconnectUnsubscribeRequest(IWampClientProxy<TMessage> client)
                {
                    mClient = client;
                }

                public IWampClientProxy<TMessage> Client
                {
                    get
                    {
                        return mClient;
                    }
                }

                public void Unsubscribed()
                {
                }
            }
        }

        private class RemoteObserver : IWampRawClient
        {
            private bool mIsOpen = false;

            private readonly IWampRawClient mClient;
            private readonly long mSessionId;

            public RemoteObserver(IWampRawClient client)
            {
                mClient = client;
                IWampClientProxy casted = mClient as IWampClientProxy;
                mSessionId = casted.Session;
            }

            public long SessionId
            {
                get
                {
                    return mSessionId;
                }
            }

            public bool IsOpen
            {
                get
                {
                    return mIsOpen;
                }
            }

            public void Open()
            {
                mIsOpen = true;
            }

            public void Message(WampMessage<object> message)
            {
                if (mIsOpen)
                {
                    mClient.Message(message);
                }
            }
        }

        private class RawTopicSubscriberBook
        {
            private readonly IDictionary<long, Subscription> mSubscriberIdToSubscriber =
                new SwapDictionary<long, Subscription>();

            private ImmutableHashSet<long> mSubscriberIds = ImmutableHashSet<long>.Empty;

            private readonly WampRawTopic<TMessage> mRawTopic;
            
            private readonly object mLock = new object();

            public RawTopicSubscriberBook(WampRawTopic<TMessage> rawTopic)
            {
                mRawTopic = rawTopic;
            }

            public bool HasSubscribers
            {
                get
                {
                    return mSubscriberIds.Count > 0;
                }
            }

            public RemoteObserver Subscribe(IWampClientProxy<TMessage> client)
            {
                Subscription subscription;

                if (mSubscriberIdToSubscriber.TryGetValue(client.Session, out subscription))
                {
                    return subscription.Observer;
                }
                else
                {
                    lock (mLock)
                    {
                        RemoteObserver result = new RemoteObserver(client);

                        mSubscriberIds = mSubscriberIds.Add(client.Session);

                        mSubscriberIdToSubscriber[client.Session] =
                            new Subscription(mRawTopic, client, result);

                        return result;                        
                    }
                }
            }

            public bool Unsubscribe(IWampClientProxy<TMessage> client)
            {
                lock (mLock)
                {
                    bool result;
                    mSubscriberIds = mSubscriberIds.Remove(client.Session);
                    result = mSubscriberIdToSubscriber.Remove(client.Session);
                    return result;
                }
            }

            public IEnumerable<RemoteObserver> GetRelevantSubscribers(PublishOptions options)
            {
                IEnumerable<long> relevantSubscriberIds = 
                    GetRelevantSubscriberIds(options);

                IEnumerable<RemoteObserver> relevantSubscribers =
                    relevantSubscriberIds.Select(GetSubscriberById)
                        .Where(x => x != null);

                return relevantSubscribers;
            }

            private RemoteObserver GetSubscriberById(long subscriberId)
            {
                Subscription subscription;

                if (mSubscriberIdToSubscriber.TryGetValue(subscriberId, out subscription))
                {
                    return subscription.Observer;
                }

                return null;
            }

            private IEnumerable<long> GetRelevantSubscriberIds(PublishOptions options)
            {
                ImmutableHashSet<long> result = mSubscriberIds;

                if (options.Eligible != null)
                {
                    result = ImmutableHashSet.Create(options.Eligible);
                }

                bool excludeMe = options.ExcludeMe ?? true;
                
                PublishOptionsExtended casted = options as PublishOptionsExtended;

                if (excludeMe && casted != null)
                {
                    result = result.Remove(casted.PublisherId);
                }

                if (options.Exclude != null)
                {
                    result = result.Except(options.Exclude);
                }

                return result;
            }
        }

        #endregion
    }
}