using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.PubSub
{
    internal class WampRawTopic<TMessage> : IWampRawTopic<TMessage>, IWampRawTopicRouterSubscriber, IDisposable
    {
        #region Data Members

        private readonly ConcurrentDictionary<long, Subscription> mSesssionIdToSubscription =
            new ConcurrentDictionary<long, Subscription>();

        private readonly IWampBinding<TMessage> mBinding; 
        private readonly IWampEventSerializer<TMessage> mSerializer;
        private readonly Subject<RemotePublication> mSubject = new Subject<RemotePublication>();
        private readonly string mTopicUri;
        private readonly IWampCustomizedSubscriptionId mCustomizedSubscriptionId;

        #endregion

        #region Constructor

        public WampRawTopic(string topicUri, IWampCustomizedSubscriptionId customizedSubscriptionId, IWampEventSerializer<TMessage> serializer, IWampBinding<TMessage> binding)
        {
            mSerializer = serializer;
            mTopicUri = topicUri;
            mBinding = binding;
            mCustomizedSubscriptionId = customizedSubscriptionId;
        }

        #endregion

        #region IRawWampTopic<TMessage> Members

        public void Event<TRaw>(IWampFormatter<TRaw> formatter, long publicationId, PublishOptions options)
        {
            Func<EventDetails, WampMessage<TMessage>> action =
                eventDetails => mSerializer.Event(SubscriptionId, publicationId, eventDetails);

            InnerEvent(options, action);
        }

        public void Event<TRaw>(IWampFormatter<TRaw> formatter, long publicationId, PublishOptions options,
                                TRaw[] arguments)
        {
            Func<EventDetails, WampMessage<TMessage>> action =
                details => mSerializer.Event(SubscriptionId,
                                             publicationId,
                                             details,
                                             arguments.Cast<object>().ToArray());

            InnerEvent(options, action);
        }

        public void Event<TRaw>(IWampFormatter<TRaw> formatter, long publicationId, PublishOptions options, TRaw[] arguments, IDictionary<string, TRaw> argumentsKeywords)
        {
            Func<EventDetails, WampMessage<TMessage>> action =
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

            if ((extendedOptions != null) && disclosePublisher)
            {
                result.Publisher = extendedOptions.PublisherId;
            }

            return result;
        }

        private void Publish(WampMessage<TMessage> message, PublishOptions options)
        {
            WampMessage<TMessage> raw = mBinding.GetRawMessage(message);
            mSubject.OnNext(new RemotePublication(raw, options));
        }

        private void InnerEvent(PublishOptions options, Func<EventDetails, WampMessage<TMessage>> action)
        {
            EventDetails details = GetDetails(options);

            WampMessage<TMessage> message = action(details);

            Publish(message, options);
        }

        public bool HasSubscribers
        {
            get
            {
                return mSubject.HasObservers;
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

            this.RaiseSubscriptionAdding(remoteSubscriber, options);

            IWampClient<TMessage> client = request.Client;

            RemoteObserver observer = new RemoteObserver(client);
            
            IDisposable disposable = mSubject.Subscribe(observer);
            
            Subscription subscription = new Subscription(this, client, disposable);

            mSesssionIdToSubscription.TryAdd(client.Session, subscription);

            request.Subscribed(this.SubscriptionId);

            observer.Open();

            this.RaiseSubscriptionAdded(remoteSubscriber, options);
        }

        public void Unsubscribe(IUnsubscribeRequest<TMessage> request)
        {
            IWampClient<TMessage> client = request.Client;

            Subscription subscription;
            
            if (mSesssionIdToSubscription.TryRemove(client.Session, out subscription))
            {
                this.RaiseSubscriptionRemoving(client.Session);

                subscription.Dispose();

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

        private class Subscription : IDisposable
        {
            private readonly WampRawTopic<TMessage> mParent;
            private readonly IWampClient<TMessage> mClient;
            private readonly IDisposable mDisposable;

            public Subscription(WampRawTopic<TMessage> parent, IWampClient<TMessage> client, IDisposable disposable)
            {
                mParent = parent;
                mClient = client;
                mDisposable = disposable;

                IWampConnectionMonitor monitor = mClient as IWampConnectionMonitor;
                monitor.ConnectionClosed += OnConnectionClosed;
            }

            private void OnConnectionClosed(object sender, EventArgs e)
            {
                mParent.Unsubscribe(new DisconnectUnsubscribeRequest(mClient));
                IWampConnectionMonitor monitor = sender as IWampConnectionMonitor;
                monitor.ConnectionClosed -= OnConnectionClosed;
            }

            public void Dispose()
            {
                mDisposable.Dispose();
            }

            private class DisconnectUnsubscribeRequest : IUnsubscribeRequest<TMessage>
            {
                private readonly IWampClient<TMessage> mClient;

                public DisconnectUnsubscribeRequest(IWampClient<TMessage> client)
                {
                    mClient = client;
                }

                public IWampClient<TMessage> Client
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

        private class RemotePublication
        {
            private readonly WampMessage<TMessage> mMessage;
            private readonly PublishOptions mOptions;

            public RemotePublication(WampMessage<TMessage> message, PublishOptions options)
            {
                mMessage = message;
                mOptions = options;
            }

            public WampMessage<TMessage> Message
            {
                get { return mMessage; }
            }

            public PublishOptions Options
            {
                get { return mOptions; }
            }
        }

        private class RemoteObserver : IObserver<RemotePublication>
        {
            private bool mOpen = false;

            private readonly IWampRawClient<TMessage> mClient;
            private readonly long mSessionId;

            public RemoteObserver(IWampRawClient<TMessage> client)
            {
                mClient = client;
                IWampClient casted = mClient as IWampClient;
                mSessionId = casted.Session;
            }

            public void OnNext(RemotePublication value)
            {
                if (mOpen)
                {
                    if (ShouldPublish(value.Options))
                    {
                        mClient.Message(value.Message);
                    }                    
                }
            }

            private bool ShouldPublish(PublishOptions options)
            {
                PublishOptionsExtended extendedOptions = 
                    options as PublishOptionsExtended;

                if (extendedOptions == null)
                {
                    return true;
                }

                bool excludeMe = extendedOptions.ExcludeMe ?? true;

                if (extendedOptions.PublisherId == mSessionId &&
                    excludeMe)
                {
                    return false;
                }

                if ((extendedOptions.Exclude != null) &&
                    (extendedOptions.Exclude.Contains(mSessionId)))
                {
                    return false;
                }

                if (extendedOptions.Eligible == null ||
                    extendedOptions.Eligible.Contains(mSessionId))
                {
                    return true;
                }

                return false;
            }

            public void Open()
            {
                mOpen = true;
            }

            public void OnError(Exception error)
            {
            }

            public void OnCompleted()
            {
            }
        }

        #endregion
    }
}