using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.PubSub
{
    public class WampTopic : IWampTopic
    {
        #region Data Members

        private readonly WampIdGenerator mGenerator = new WampIdGenerator();

        private readonly Subject<IPublication> mSubject = new Subject<IPublication>();
        
        private readonly string mTopicUri;

        private readonly bool mPersistent;

        public WampTopic(string topicUri, bool persistent)
        {
            mTopicUri = topicUri;
            mPersistent = persistent;
        }

        #endregion

        #region IWampTopic members

        public bool HasSubscribers
        {
            get
            {
                return mSubject.HasObservers;
            }
        }

        public string TopicUri
        {
            get
            {
                return mTopicUri;
            }
        }

        public long Publish<TMessage>(IWampFormatter<TMessage> formatter, PublishOptions options)
        {
            Action<IWampRawTopicRouterSubscriber, long> publishAction =
                (subscriber, publicationId) => subscriber.Event(formatter, publicationId, options);

            return InnerPublish(publishAction);
        }

        public long Publish<TMessage>(IWampFormatter<TMessage> formatter, PublishOptions options, TMessage[] arguments)
        {
            Action<IWampRawTopicRouterSubscriber, long> publishAction =
                (subscriber, publicationId) => subscriber.Event(formatter, publicationId, options, arguments);

            return InnerPublish(publishAction);
        }

        public long Publish<TMessage>(IWampFormatter<TMessage> formatter, PublishOptions options, TMessage[] arguments, TMessage argumentKeywords)
        {
            Action<IWampRawTopicRouterSubscriber, long> publishAction =
                (subscriber, publicationId) => subscriber.Event(formatter, publicationId, options, arguments, argumentKeywords);

            return InnerPublish(publishAction);
        }


        public bool Persistent
        {
            get
            {
                return mPersistent;
            }
        }

        public IDisposable Subscribe(IWampRawTopicRouterSubscriber subscriber)
        {
            RegisterSubscriberEventsIfNeeded(subscriber);

            IDisposable subscriptionDisposable = 
                mSubject.Subscribe(new SubscriberObserver(subscriber));

            IDisposable result = subscriptionDisposable;

            result = new CompositeDisposable(subscriptionDisposable, 
                Disposable.Create(() => OnSubscriberLeave(subscriber)));

            return result;
        }

        private void OnSubscriberLeave(IWampRawTopicRouterSubscriber subscriber)
        {
            UnregisterSubscriberEventsIfNeeded(subscriber);

            if (!mSubject.HasObservers)
            {
                RaiseTopicEmpty();
            }
        }

        public void Dispose()
        {
            mSubject.Dispose();
        }

        #endregion

        #region ISubscriptionNotifier

        public event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdding;

        public event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdded;

        public event EventHandler<SubscriptionRemoveEventArgs> SubscriptionRemoving;

        public event EventHandler<SubscriptionRemoveEventArgs> SubscriptionRemoved;

        public event EventHandler TopicEmpty;

        #endregion

        #region Event forwarding

        private void RegisterSubscriberEventsIfNeeded(IWampRawTopicRouterSubscriber subscriber)
        {
            ISubscriptionNotifier notifier = subscriber as ISubscriptionNotifier;

            if (notifier != null)
            {
                RegisterSubscriberEvents(notifier);
            }
        }

        private void RegisterSubscriberEvents(ISubscriptionNotifier notifier)
        {
            notifier.SubscriptionAdded += OnSubscriberSubscriptionAdded;
            notifier.SubscriptionAdding += OnSubscriberSubscriptionAdding;
            notifier.SubscriptionRemoved += OnSubscriberSubscriptionRemoved;
            notifier.SubscriptionRemoving += OnSubscriberSubscriptionRemoving;
        }

        private void UnregisterSubscriberEventsIfNeeded(IWampRawTopicRouterSubscriber subscriber)
        {
            ISubscriptionNotifier subscriptionNotifier = subscriber as ISubscriptionNotifier;

            if (subscriptionNotifier != null)
            {
                UnregisterSubscriberEvents(subscriptionNotifier);
            }
        }

        private void UnregisterSubscriberEvents(ISubscriptionNotifier notifier)
        {
            notifier.SubscriptionRemoving -= OnSubscriberSubscriptionRemoving;
            notifier.SubscriptionRemoved -= OnSubscriberSubscriptionRemoved;
            notifier.SubscriptionAdding -= OnSubscriberSubscriptionAdding;
            notifier.SubscriptionAdded -= OnSubscriberSubscriptionAdded;
        }

        private void OnSubscriberSubscriptionAdded(object sender, WampSubscriptionAddEventArgs e)
        {
            RaiseSubscriptionAdded(e);
        }

        private void OnSubscriberSubscriptionAdding(object sender, WampSubscriptionAddEventArgs e)
        {
            RaiseSubscriptionAdding(e);
        }

        private void OnSubscriberSubscriptionRemoved(object sender, SubscriptionRemoveEventArgs e)
        {
            RaiseSubscriptionRemoved(e);
        }

        private void OnSubscriberSubscriptionRemoving(object sender, SubscriptionRemoveEventArgs e)
        {
            RaiseSubscriptionRemoving(e);
        }

        #endregion

        #region Private methods

        private long InnerPublish(Action<IWampRawTopicRouterSubscriber, long> publishAction)
        {
            long publicationId = mGenerator.Generate();

            mSubject.OnNext
                (new Publication(publishAction, publicationId));

            return publicationId;
        }

        protected virtual void RaiseTopicEmpty()
        {
            EventHandler handler = TopicEmpty;
            
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void RaiseSubscriptionAdding(WampSubscriptionAddEventArgs e)
        {
            EventHandler<WampSubscriptionAddEventArgs> handler = SubscriptionAdding;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void RaiseSubscriptionAdded(WampSubscriptionAddEventArgs e)
        {
            EventHandler<WampSubscriptionAddEventArgs> handler = SubscriptionAdded;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void RaiseSubscriptionRemoving(SubscriptionRemoveEventArgs e)
        {
            EventHandler<SubscriptionRemoveEventArgs> handler = SubscriptionRemoving;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void RaiseSubscriptionRemoved(SubscriptionRemoveEventArgs e)
        {
            EventHandler<SubscriptionRemoveEventArgs> handler = SubscriptionRemoved;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Nested Classes

        private class SubscriberObserver : IObserver<IPublication>
        {
            private readonly IWampRawTopicRouterSubscriber mSubscriber;

            public SubscriberObserver(IWampRawTopicRouterSubscriber subscriber)
            {
                mSubscriber = subscriber;
            }

            public void OnNext(IPublication publication)
            {
                publication.Publish(mSubscriber);
            }

            public void OnError(Exception error)
            {
            }

            public void OnCompleted()
            {
            }
        }

        private interface IPublication
        {
            void Publish(IWampRawTopicRouterSubscriber subscriber);
        }

        private class Publication : IPublication
        {
            private readonly Action<IWampRawTopicRouterSubscriber, long> mPublishAction;
            private readonly long mPublicationId;

            public Publication(Action<IWampRawTopicRouterSubscriber, long> publishAction, long publicationId)
            {
                mPublishAction = publishAction;
                mPublicationId = publicationId;
            }

            public void Publish(IWampRawTopicRouterSubscriber subscriber)
            {
                mPublishAction(subscriber, mPublicationId);
            }
        }

        #endregion
    }
}