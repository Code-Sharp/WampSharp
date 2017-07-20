using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class RetainSubscriber : IWampRawTopicWeakRouterSubscriber
    {
        public RetainSubscriber(IWampTopic topic)
        {            
            topic.SubscriptionAdded += OnSubscriptionAdded;
        }

        private void OnSubscriptionAdded(object sender, WampSubscriptionAddEventArgs e)
        {
            if (e.Options.GetRetained == true)
            {
                e.Subscriber.Event(new EventDetails());
            }
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options)
        {
            throw new System.NotImplementedException();
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options, TMessage[] arguments)
        {
            throw new System.NotImplementedException();
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions options, TMessage[] arguments,
                                    IDictionary<string, TMessage> argumentsKeywords)
        {
            throw new System.NotImplementedException();
        }
    }
}