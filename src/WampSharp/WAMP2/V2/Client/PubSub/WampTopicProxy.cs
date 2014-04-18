using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Client
{
    internal class WampTopicProxy<TMessage> : IWampTopicProxy
    {
        private bool mDisposed = false;
        private readonly string mTopicUri;
        private readonly WampSubscriber<TMessage> mSubscriber;
        private readonly WampPublisher<TMessage> mPublisher;
        private readonly IDisposable mConatinerDisposable;

        private readonly object mLock = new object();
        
        private ConcurrentDictionary<object, WampTopicProxySection<TMessage>> mOptionsToSection =
            new ConcurrentDictionary<object, WampTopicProxySection<TMessage>>();

        public WampTopicProxy(string topicUri,
                              WampSubscriber<TMessage> subscriber,
                              WampPublisher<TMessage> publisher,
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
            lock (mLock)
            {
                CheckDisposed();

                WampTopicProxySection<TMessage> section = GetSection(options);

                Task<IDisposable> task = section.Subscribe(subscriber);

                return task;                
            }
        }

        private WampTopicProxySection<TMessage> GetSection(object options)
        {
            return mOptionsToSection.GetOrAdd(options, x => CreateSection(options));
        }

        private WampTopicProxySection<TMessage> CreateSection(object options)
        {
            WampTopicProxySection<TMessage> result =
                new WampTopicProxySection<TMessage>(TopicUri, mSubscriber, options);

            result.SectionEmpty += OnSectionEmpty;

            return result;
        }

        private void OnSectionEmpty(object sender, EventArgs e)
        {
            WampTopicProxySection<TMessage> section = sender as WampTopicProxySection<TMessage>;

            lock (mLock)
            {
                if (!section.HasSubscribers)
                {
                    section.SectionEmpty -= OnSectionEmpty;
                    WampTopicProxySection<TMessage> removed;
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
    }
}