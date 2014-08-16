using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            Task<IDisposable> task = mTopic.Subscribe(new RawSubscriber(observer), new {});
            
            // TODO: think of a better solution
            task.Wait();
            
            return task;
        }
    }
}