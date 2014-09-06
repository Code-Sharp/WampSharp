using System;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public interface IWampTopic : ISubscriptionNotifier, IDisposable
    {
        bool HasSubscribers { get; }

        string TopicUri { get; }

        long Publish<TMessage>(IWampFormatter<TMessage> formatter, PublishOptions publishOptions);
        long Publish<TMessage>(IWampFormatter<TMessage> formatter, PublishOptions publishOptions, TMessage[] arguments);
        long Publish<TMessage>(IWampFormatter<TMessage> formatter, PublishOptions publishOptions, TMessage[] arguments, TMessage argumentKeywords);

        IDisposable Subscribe(IWampRawTopicRouterSubscriber subscriber);
    }
}