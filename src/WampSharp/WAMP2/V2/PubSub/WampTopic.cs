using System;
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

        public WampTopic(string topicUri)
        {
            mTopicUri = topicUri;
        }

        #endregion

        #region IWampTopic members

        public string TopicUri
        {
            get
            {
                return mTopicUri;
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
            return mSubject.Subscribe(new SubscriberObserver(subscriber));
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