using System;
using System.Collections.Generic;
using WampSharp.V2.Core;
using WampSharp.V2.PubSub;

namespace WampSharp.V2
{
    internal class WampRouterSubject : WampSubject
    {
        private readonly IWampTopic mTopic;

        public WampRouterSubject(IWampTopic topic)
        {
            mTopic = topic;
        }

        public override IDisposable Subscribe(IObserver<IWampSerializedEvent> observer)
        {
            return mTopic.Subscribe(new RawSubscriber(observer), null);
        }

        protected override void Publish(IDictionary<string, object> options)
        {
            mTopic.Publish(WampObjectFormatter.Value,
                           options);
        }

        protected override void Publish(IDictionary<string, object> options, object[] arguments)
        {
            mTopic.Publish(WampObjectFormatter.Value,
                           options,
                           arguments);
        }

        protected override void Publish(IDictionary<string, object> options, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            mTopic.Publish(WampObjectFormatter.Value,
                           options,
                           arguments,
                           argumentsKeywords);
        }
    }
}