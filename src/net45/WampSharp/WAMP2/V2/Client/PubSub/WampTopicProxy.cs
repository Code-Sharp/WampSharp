using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Client
{
    internal class WampTopicProxy : IWampTopicProxy
    {
        private bool mDisposed = false;
        private readonly string mTopicUri;
        private readonly IWampTopicSubscriptionProxy mSubscriber;
        private readonly IWampTopicPublicationProxy mPublisher;
        private readonly IDisposable mContainerDisposable;

        private readonly object mLock = new object();

        private ConcurrentDictionary<SubscribeOptions, WampTopicClientProxySection> mOptionsToSection =
            new ConcurrentDictionary<SubscribeOptions, WampTopicClientProxySection>();

        public WampTopicProxy(string topicUri,
                              IWampTopicSubscriptionProxy subscriber,
                              IWampTopicPublicationProxy publisher,
                              IDisposable containerDisposable)
        {
            mTopicUri = topicUri;
            mSubscriber = subscriber;
            mPublisher = publisher;
            mContainerDisposable = containerDisposable;
        }

        public string TopicUri
        {
            get
            {
                return mTopicUri;
            }
        }

        public Task<long?> Publish(PublishOptions options)
        {
            CheckDisposed();

            return mPublisher.Publish(this.TopicUri, options);
        }

        public Task<long?> Publish(PublishOptions options, object[] arguments)
        {
            CheckDisposed();

            return mPublisher.Publish(this.TopicUri, options, arguments);
        }

        public Task<long?> Publish(PublishOptions options, object[] arguments, IDictionary<string, object> argumentKeywords)
        {
            CheckDisposed();

            return mPublisher.Publish(this.TopicUri, options, arguments, argumentKeywords);
        }

        public Task<IDisposable> Subscribe(IWampRawTopicClientSubscriber subscriber, SubscribeOptions options)
        {
            lock (mLock)
            {
                CheckDisposed();

                WampTopicClientProxySection section = GetSection(options);

                Task<IDisposable> task = section.Subscribe(subscriber);

                return task;
            }
        }


        private WampTopicClientProxySection GetSection(SubscribeOptions options)
        {
            return mOptionsToSection.GetOrAdd(options, x => CreateSection(options));
        }

        private WampTopicClientProxySection CreateSection(SubscribeOptions options)
        {
            WampTopicClientProxySection result =
                new WampTopicClientProxySection(TopicUri, mSubscriber, options);

            result.SectionEmpty += OnSectionEmpty;

            return result;
        }

        private void OnSectionEmpty(object sender, EventArgs e)
        {
            WampTopicClientProxySection section = sender as WampTopicClientProxySection;

            lock (mLock)
            {
                if (!section.HasSubscribers)
                {
                    section.SectionEmpty -= OnSectionEmpty;
                    WampTopicClientProxySection removed;
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

                        mContainerDisposable.Dispose();
                        
                        mDisposed = true;
                    }
                }                
            }
        }
    }
}