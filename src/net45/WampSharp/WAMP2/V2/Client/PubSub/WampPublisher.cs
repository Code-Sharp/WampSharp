using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Client
{
    internal class WampPublisher<TMessage> : IWampTopicPublicationProxy,
                                             IWampPublisher<TMessage>, IWampPublisherError<TMessage>
    {
        private readonly IWampFormatter<TMessage> mFormatter; 
        private readonly IWampServerProxy mProxy;
        private readonly IWampClientConnectionMonitor mMonitor;
        private readonly WampRequestIdMapper<Publication> mPendingPublication = new WampRequestIdMapper<Publication>(); 

        public WampPublisher(IWampServerProxy proxy, IWampFormatter<TMessage> formatter, IWampClientConnectionMonitor monitor)
        {
            mProxy = proxy;
            mMonitor = monitor;
            mFormatter = formatter;

            mMonitor.ConnectionBroken += ConnectionBroken;
            mMonitor.ConnectionError += ConnectionError;
        }

        private bool IsConnected
        {
            get { return mMonitor.IsConnected; }
        }

        public Task<long?> Publish(string topicUri, PublishOptions options)
        {
            return InnerPublish(options,
                                () => new Publication(mFormatter, topicUri, options),
                                requestId => mProxy.Publish(requestId, options, topicUri));
        }

        public Task<long?> Publish(string topicUri, PublishOptions options, object[] arguments)
        {
            return InnerPublish(options,
                                () => new Publication(mFormatter, topicUri, options, arguments),
                                requestId => mProxy.Publish(requestId, options, topicUri, arguments));
        }

        public Task<long?> Publish(string topicUri, PublishOptions options, object[] arguments, IDictionary<string, object> argumentKeywords)
        {
            return InnerPublish(options,
                                () => new Publication(mFormatter, topicUri, options, arguments, argumentKeywords),
                                requestId => mProxy.Publish(requestId, options, topicUri, arguments, argumentKeywords));
        }

        private Task<long?> InnerPublish(PublishOptions options, Func<Publication> publicationFactory, Action<long> publicationAction)
        {
            if (!IsConnected)
            {
                throw new WampSessionNotEstablishedException();
            }

            Publication publication = publicationFactory();

            long requestId = mPendingPublication.Add(publication);

            publication.RequestId = requestId;

            publicationAction(requestId);

            bool acknowledge = options.Acknowledge ?? false;
            if (!acknowledge)
            {
                Publication removed;
                mPendingPublication.TryRemove(requestId, out removed);
#if NET45
                return Task.FromResult(default(long?));
#elif NET40
                TaskCompletionSource<long?> result = new TaskCompletionSource<long?>();
                result.SetResult(default(long?));
                return result.Task;
#endif
            }

            return publication.Task;
        }

        public void Published(long requestId, long publicationId)
        {
            Publication publication;
            
            if (mPendingPublication.TryRemove(requestId, out publication))
            {
                publication.Complete(publicationId);
            }
        }

        public void PublishError(long requestId, TMessage details, string error)
        {
            Publication publication;

            if (mPendingPublication.TryRemove(requestId, out publication))
            {
                publication.Error(details, error);
            }
        }

        public void PublishError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            Publication publication;

            if (mPendingPublication.TryRemove(requestId, out publication))
            {
                publication.Error(details, error, arguments);
            }
        }

        public void PublishError(long requestId, TMessage details, string error, TMessage[] arguments,
                                 TMessage argumentsKeywords)
        {
            Publication publication;

            if (mPendingPublication.TryRemove(requestId, out publication))
            {
                publication.Error(details, error, arguments, argumentsKeywords);
            }
        }

        private void ConnectionError(object sender, WampConnectionErrorEventArgs e)
        {
            mPendingPublication.ConnectionError(e.Exception);
        }

        private void ConnectionBroken(object sender, WampSessionCloseEventArgs e)
        {
            mPendingPublication.ConnectionClosed(e);
        }

        private class Publication : WampPendingRequest<TMessage, long?>
        {
            private readonly string mTopicUri;
            private readonly PublishOptions mOptions;
            private readonly object[] mArguments;
            private readonly IDictionary<string, object> mArgumentKeywords;

            public Publication(IWampFormatter<TMessage> formatter, 
                string topicUri, 
                PublishOptions options, 
                object[] arguments = null, 
                IDictionary<string, object> argumentKeywords = null) : 
                base(formatter)
            {
                mTopicUri = topicUri;
                mOptions = options;
                mArguments = arguments;
                mArgumentKeywords = argumentKeywords;
            }

            public string TopicUri
            {
                get
                {
                    return mTopicUri;
                }
            }

            public PublishOptions Options
            {
                get
                {
                    return mOptions;
                }
            }

            public object[] Arguments
            {
                get
                {
                    return mArguments;
                }
            }

            public IDictionary<string, object> ArgumentKeywords
            {
                get
                {
                    return mArgumentKeywords;
                }
            }
        }
    }
}