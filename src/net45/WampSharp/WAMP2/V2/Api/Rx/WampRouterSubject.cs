using System;
using System.Collections.Generic;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2
{
    internal class WampRouterSubject : WampSubject
    {
        private readonly string mTopicUri;
        private readonly IWampTopicContainer mContainer;

        public WampRouterSubject(string topicUri, IWampTopicContainer container)
        {
            mTopicUri = topicUri;
            mContainer = container;
        }

        public override IDisposable Subscribe(IObserver<IWampSerializedEvent> observer)
        {
            return mContainer.Subscribe(new RawRouterSubscriber(observer), mTopicUri);
        }

        protected override void Publish(PublishOptions options)
        {
            mContainer.Publish(WampObjectFormatter.Value,
                               options,
                               mTopicUri);
        }

        protected override void Publish(PublishOptions options, object[] arguments)
        {
            mContainer.Publish(WampObjectFormatter.Value,
                               options,
                               mTopicUri,
                               arguments);
        }

        protected override void Publish(PublishOptions options, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            mContainer.Publish(WampObjectFormatter.Value,
                               options,
                               mTopicUri,
                               arguments,
                               argumentsKeywords);
        }
    }
}