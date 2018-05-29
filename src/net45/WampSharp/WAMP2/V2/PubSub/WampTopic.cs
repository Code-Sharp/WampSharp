using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class WampTopic : IWampTopic
    {
        #region Data Members

        private readonly SwapCollection<IWampRawTopicRouterSubscriber> mSubscribers =
            new SwapCollection<IWampRawTopicRouterSubscriber>();
        private readonly bool mPersistent;

        public WampTopic(string topicUri, bool persistent)
        {
            TopicUri = topicUri;
            mPersistent = persistent;
        }

        #endregion

        #region IWampTopic members

        public bool HasSubscribers => mSubscribers.Count > 0;

        public string TopicUri { get; }

        public void Publish<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options)
        {
            Action<IWampRawTopicRouterSubscriber> publishAction =
                (subscriber) => subscriber.Event(formatter, publicationId, options);

            InnerPublish(publishAction);
        }

        public void Publish<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options, TMessage[] arguments)
        {
            Action<IWampRawTopicRouterSubscriber> publishAction =
                (subscriber) => subscriber.Event(formatter, publicationId, options, arguments);

            InnerPublish(publishAction);
        }

        public void Publish<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options, TMessage[] arguments, IDictionary<string, TMessage> argumentKeywords)
        {
            Action<IWampRawTopicRouterSubscriber> publishAction =
                (subscriber) => subscriber.Event(formatter, publicationId, options, arguments, argumentKeywords);

            InnerPublish(publishAction);
        }

        public bool Persistent => mPersistent;

        public long SubscriptionId
        {
            get;
            set;
        }

        public IDisposable Subscribe(IWampRawTopicRouterSubscriber subscriber)
        {
            RegisterSubscriberEventsIfNeeded(subscriber);

            mSubscribers.Add(subscriber);

            IDisposable result = Disposable.Create(() =>
            {
                mSubscribers.Remove(subscriber);
                OnSubscriberLeave(subscriber);
            });

            return result;
        }

        private void OnSubscriberLeave(IWampRawTopicRouterSubscriber subscriber)
        {
            UnregisterSubscriberEventsIfNeeded(subscriber);

            if (mSubscribers.Count == 0)
            {
                RaiseTopicEmpty();
            }
        }

        public void Dispose()
        {
            mSubscribers.Clear();
        }

        #endregion

        #region ISubscriptionNotifier

        public event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdding;

        public event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdded;

        public event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoving;

        public event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoved;

        public event EventHandler TopicEmpty;

        #endregion

        #region Event forwarding

        private void RegisterSubscriberEventsIfNeeded(IWampRawTopicRouterSubscriber subscriber)
        {

            if (subscriber is ISubscriptionNotifier notifier)
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

            if (subscriber is ISubscriptionNotifier subscriptionNotifier)
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

        private void OnSubscriberSubscriptionRemoved(object sender, WampSubscriptionRemoveEventArgs e)
        {
            RaiseSubscriptionRemoved(e);
        }

        private void OnSubscriberSubscriptionRemoving(object sender, WampSubscriptionRemoveEventArgs e)
        {
            RaiseSubscriptionRemoving(e);
        }

        #endregion

        #region Private methods

        private void InnerPublish(Action<IWampRawTopicRouterSubscriber> publishAction)
        {
            foreach (IWampRawTopicRouterSubscriber subscriber in mSubscribers)
            {
                publishAction(subscriber);
            }
        }

        protected virtual void RaiseTopicEmpty()
        {
            TopicEmpty?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void RaiseSubscriptionAdding(WampSubscriptionAddEventArgs e)
        {
            SubscriptionAdding?.Invoke(this, e);
        }

        protected virtual void RaiseSubscriptionAdded(WampSubscriptionAddEventArgs e)
        {
            SubscriptionAdded?.Invoke(this, e);
        }

        protected virtual void RaiseSubscriptionRemoving(WampSubscriptionRemoveEventArgs e)
        {
            SubscriptionRemoving?.Invoke(this, e);
        }

        protected virtual void RaiseSubscriptionRemoved(WampSubscriptionRemoveEventArgs e)
        {
            SubscriptionRemoved?.Invoke(this, e);
        }

        #endregion
    }
}