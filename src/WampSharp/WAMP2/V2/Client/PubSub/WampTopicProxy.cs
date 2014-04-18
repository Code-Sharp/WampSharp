using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Client
{
    internal class WampTopicProxy : IWampTopicProxy
    {
        private bool mDisposed = false;
        private readonly string mTopicUri;
        private readonly IWampTopicSubscriptionProxy mSubscriber;
        private readonly IWampTopicPublicationProxy mPublisher;
        private readonly IDisposable mConatinerDisposable;

        private readonly object mLock = new object();
        
        private ConcurrentDictionary<object, WampTopicProxySection> mOptionsToSection =
            new ConcurrentDictionary<object, WampTopicProxySection>();

        public WampTopicProxy(string topicUri,
                              IWampTopicSubscriptionProxy subscriber,
                              IWampTopicPublicationProxy publisher,
                              IDisposable conatinerDisposable)
        {
            mTopicUri = topicUri;
            mSubscriber = subscriber;
            mPublisher = publisher;
            mConatinerDisposable = conatinerDisposable;
        }

        public string TopicUri
        {
            get
            {
                return mTopicUri;
            }
        }

        public Task<long> Publish(object options)
        {
            CheckDisposed();

            return mPublisher.Publish(this.TopicUri, options);
        }

        public Task<long> Publish(object options, object[] arguments)
        {
            CheckDisposed();

            return mPublisher.Publish(this.TopicUri, options, arguments);
        }

        public Task<long> Publish(object options, object[] arguments, object argumentKeywords)
        {
            CheckDisposed();

            return mPublisher.Publish(this.TopicUri, options, arguments, argumentKeywords);
        }

        public Task<IDisposable> Subscribe(IWampTopicSubscriber subscriber, object options)
        {
            RawSubscriberAdapter rawSubscriberAdapter = new RawSubscriberAdapter(subscriber);
            return Subscribe(rawSubscriberAdapter, options);
        }

        public Task<IDisposable> Subscribe(IWampRawTopicSubscriber subscriber, object options)
        {
            lock (mLock)
            {
                CheckDisposed();

                WampTopicProxySection section = GetSection(options);

                Task<IDisposable> task = section.Subscribe(subscriber);

                return task;
            }
        }


        private WampTopicProxySection GetSection(object options)
        {
            return mOptionsToSection.GetOrAdd(options, x => CreateSection(options));
        }

        private WampTopicProxySection CreateSection(object options)
        {
            WampTopicProxySection result =
                new WampTopicProxySection(TopicUri, mSubscriber, options);

            result.SectionEmpty += OnSectionEmpty;

            return result;
        }

        private void OnSectionEmpty(object sender, EventArgs e)
        {
            WampTopicProxySection section = sender as WampTopicProxySection;

            lock (mLock)
            {
                if (!section.HasSubscribers)
                {
                    section.SectionEmpty -= OnSectionEmpty;
                    WampTopicProxySection removed;
                    mOptionsToSection.TryRemove(section.Options, out removed);
                    section.Dispose();
                }
            }
        }

        private void CheckDisposed()
        {
            if (mDisposed)
            {
                throw new ObjectDisposedException("This topic proxy was already been disposed");
            }
        }

        public void Dispose()
        {
            if (!mDisposed)
            {
                lock (mLock)
                {
                    if (!mDisposed)
                    {
                        foreach (var keyValuePair in mOptionsToSection)
                        {
                            keyValuePair.Value.Dispose();
                        }

                        mOptionsToSection = null;

                        mConatinerDisposable.Dispose();
                        
                        mDisposed = true;
                    }
                }                
            }
        }

        private class RawSubscriberAdapter : IWampRawTopicSubscriber
        {
            private readonly IWampTopicSubscriber mSubscriber;

            public RawSubscriberAdapter(IWampTopicSubscriber subscriber)
            {
                mSubscriber = subscriber;
            }

            public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details)
            {
                mSubscriber.Event(publicationId, details);
            }

            public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details, TMessage[] arguments)
            {
                mSubscriber.Event(publicationId, details, arguments.Cast<object>().ToArray());
            }

            public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, TMessage details, TMessage[] arguments,
                                        TMessage argumentsKeywords)
            {
                mSubscriber.Event(publicationId, details, arguments.Cast<object>().ToArray(), argumentsKeywords);
            }
        }
    }
}