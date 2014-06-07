using System;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.PubSub
{
    public interface IWampTopic : ISubscriptionNotifier, IDisposable
    {
        bool HasSubscribers { get; }

        string TopicUri { get; }

        long Publish<TMessage>(IWampFormatter<TMessage> formatter, TMessage options);
        long Publish<TMessage>(IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments);
        long Publish<TMessage>(IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments, TMessage argumentKeywords);

        IDisposable Subscribe(IWampRawTopicSubscriber subscriber, object options);
    }
}