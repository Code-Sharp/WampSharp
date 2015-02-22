using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemEx;
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

        public Task<IAsyncDisposable> Subscribe(IWampRawTopicClientSubscriber subscriber, SubscribeOptions options)
        {
            lock (mLock)
            {
                CheckDisposed();

                Task<IAsyncDisposable> task = 
                    mSubscriber.Subscribe(subscriber, options, this.TopicUri);

                return task;
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
                        mContainerDisposable.Dispose();
                        
                        mDisposed = true;
                    }
                }                
            }
        }
    }
}