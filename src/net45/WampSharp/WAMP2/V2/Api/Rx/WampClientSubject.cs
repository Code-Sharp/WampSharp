using System;
using System.Collections.Generic;
using WampSharp.V2.Client;

namespace WampSharp.V2
{
    internal class WampClientSubject : WampSubject
    {
        private readonly IWampTopicProxy mTopic;

        public WampClientSubject(IWampTopicProxy topic)
        {
            mTopic = topic;
        }

        protected override void Publish(IDictionary<string, object> options)
        {
            mTopic.Publish(options);
        }

        protected override void Publish(IDictionary<string, object> options, object[] arguments)
        {
            mTopic.Publish(options, arguments);
        }

        protected override void Publish(IDictionary<string, object> options, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            mTopic.Publish(options, arguments, argumentsKeywords);
        }

        public override IDisposable Subscribe(IObserver<IWampSerializedEvent> observer)
        {
            return mTopic.Subscribe(new RawSubscriber(observer), new {});
        }
    }
}