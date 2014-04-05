using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using WampSharp.V2.Core;
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

        public bool Persistent
        {
            get
            {
                return mPersistent;
            }
        }

        public long Publish(object options)
        {
            Action<IWampTopicSubscriber, long> publishAction = 
                (subscriber, publicationId) => subscriber.Event(publicationId, options);
            
            return InnerPublish(publishAction);
        }

        public long Publish(object options, object[] arguments)
        {
            Action<IWampTopicSubscriber, long> publishAction =
                (subscriber, publicationId) => subscriber.Event(publicationId, options, arguments);

            return InnerPublish(publishAction);
        }

        public long Publish(object options, object[] arguments, object argumentKeywords)
        {
            Action<IWampTopicSubscriber, long> publishAction =
                (subscriber, publicationId) => subscriber.Event(publicationId, options, arguments, argumentKeywords);

            return InnerPublish(publishAction);
        }

        public IDisposable Subscribe(IWampTopicSubscriber subscriber, object options)
        {
            RegisterSubscriberEventsIfNeeded(subscriber);

            IDisposable subscriptionDisposable = 
                mSubject.Subscribe(new SubscriberObserver(subscriber));

            IDisposable result = subscriptionDisposable;

            result = new CompositeDisposable(subscriptionDisposable, 
                Disposable.Create(() => OnSubscriberLeave(subscriber)));

            return result;
        }

        private void OnSubscriberLeave(IWampTopicSubscriber subscriber)
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

        public event EventHandler<SubscriptionAddEventArgs> SubscriptionAdding;

        public event EventHandler<SubscriptionAddEventArgs> SubscriptionAdded;

        public event EventHandler<SubscriptionRemoveEventArgs> SubscriptionRemoving;

        public event EventHandler<SubscriptionRemoveEventArgs> SubscriptionRemoved;

        public event EventHandler TopicEmpty;

        #endregion

        #region Event forwarding

        private void RegisterSubscriberEventsIfNeeded(IWampTopicSubscriber subscriber)
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

        private void UnregisterSubscriberEventsIfNeeded(IWampTopicSubscriber subscriber)
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

        private void OnSubscriberSubscriptionAdded(object sender, SubscriptionAddEventArgs e)
        {
            RaiseSubscriptionAdded(e);
        }

        private void OnSubscriberSubscriptionAdding(object sender, SubscriptionAddEventArgs e)
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

        private long InnerPublish(Action<IWampTopicSubscriber, long> publishAction)
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

        protected virtual void RaiseSubscriptionAdding(SubscriptionAddEventArgs e)
        {
            EventHandler<SubscriptionAddEventArgs> handler = SubscriptionAdding;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void RaiseSubscriptionAdded(SubscriptionAddEventArgs e)
        {
            EventHandler<SubscriptionAddEventArgs> handler = SubscriptionAdded;

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
            private readonly IWampTopicSubscriber mSubscriber;

            public SubscriberObserver(IWampTopicSubscriber subscriber)
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
            void Publish(IWampTopicSubscriber subscriber);
        }

        private class Publication : IPublication
        {
            private readonly Action<IWampTopicSubscriber, long> mPublishAction;
            private readonly long mPublicationId;

            public Publication(Action<IWampTopicSubscriber, long> publishAction, long publicationId)
            {
                mPublishAction = publishAction;
                mPublicationId = publicationId;
            }

            public void Publish(IWampTopicSubscriber subscriber)
            {
                mPublishAction(subscriber, mPublicationId);
            }
        }

        #endregion
    }
}