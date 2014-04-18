using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Client
{
    internal class WampTopicProxySection : IWampRawTopicSubscriber, IDisposable
    {
        private int mSubscribed = 0;
        private readonly IWampTopicSubscriptionProxy mSubscriber;
        private Subject<IPublication> mSubject = new Subject<IPublication>();
        private readonly string mTopicUri;
        private readonly object mOptions;
        private readonly object mLock = new object();
        private IDisposable mExternalSubscription;

        public WampTopicProxySection(string topicUri, IWampTopicSubscriptionProxy subscriber, object options)
        {
            mTopicUri = topicUri;
            mSubscriber = subscriber;
            mOptions = options;
        }

        public string TopicUri
        {
            get
            {
                return mTopicUri;
            }
        }

        public object Options
        {
            get
            {
                return mOptions;
            }
        }

        public bool HasSubscribers
        {
            get
            {
                return mSubject.HasObservers;
            }
        }

        public event EventHandler SectionEmpty;

        public Task<IDisposable> Subscribe(IWampRawTopicSubscriber subscriber)
        {
            object options = Options;
            Task<IDisposable> externalSubscription = SubscribeExternal(options);

            IDisposable disposable =
                SubscribeInternal(subscriber);

            if (externalSubscription == null)
            {
                return Task.FromResult(disposable);
            }
            else
            {
                return externalSubscription.
                    ContinueWith(x =>
                                     {
                                         mExternalSubscription = x.Result;
                                         return disposable;
                                     });
            }
        }

        private IDisposable SubscribeInternal(IWampRawTopicSubscriber subscriber)
        {
            IDisposable result = 
                mSubject.Subscribe(new TopicSubscriberObserver(subscriber));

            return new CompositeDisposable(result,
                                           Disposable.Create(() => OnSubscriberRemoved(subscriber)));
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details)
        {
            PublishInternal(subscriber =>
                subscriber.Event(formatter, publicationId, details));
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details, TMessage[] arguments)
        {
            PublishInternal(subscriber =>
                            subscriber.Event(formatter,
                                             publicationId,
                                             details,
                                             arguments));
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details, TMessage[] arguments,
                                    TMessage argumentsKeywords)
        {
            PublishInternal(subscriber =>
                            subscriber.Event(formatter,
                                             publicationId,
                                             details,
                                             arguments,
                                             argumentsKeywords));
        }
        
        private void OnSubscriberRemoved(IWampRawTopicSubscriber subscriber)
        {
            if (!HasSubscribers)
            {
                RaiseSectionEmpty();
            }
        }

        private Task<IDisposable> SubscribeExternal(object options)
        {
            if (Interlocked.CompareExchange(ref mSubscribed, 1, 0) == 0)
            {
                return mSubscriber.Subscribe(this, options, TopicUri);
            }

            return null;
        }

        private void RaiseSectionEmpty()
        {
            EventHandler handler = SectionEmpty;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void PublishInternal(Action<IWampRawTopicSubscriber> action)
        {
            mSubject.OnNext(new Publication(action));
        }

        public void Dispose()
        {
            lock (mLock)
            {
                if (mSubject != null)
                {
                    mSubject.Dispose();
                    mSubject = null;
                }

                if (mExternalSubscription != null)
                {
                    mExternalSubscription.Dispose();
                    mExternalSubscription = null;
                }
            }
        }

        private interface IPublication
        {
            void Publish(IWampRawTopicSubscriber subscriber);
        }

        private class Publication : IPublication
        {
            private readonly Action<IWampRawTopicSubscriber> mAction;

            public Publication(Action<IWampRawTopicSubscriber> action)
            {
                mAction = action;
            }

            public void Publish(IWampRawTopicSubscriber subscriber)
            {
                mAction(subscriber);
            }
        }

        private class TopicSubscriberObserver : IObserver<IPublication>
        {
            private readonly IWampRawTopicSubscriber mSubscriber;

            public TopicSubscriberObserver(IWampRawTopicSubscriber subscriber)
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
    }
}