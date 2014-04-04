using System;

namespace WampSharp.V2.PubSub
{
    public interface IWampTopic
    {
        string TopicUri { get; }

        long Publish(object options);
        long Publish(object options, object[] arguments);
        long Publish(object options, object[] arguments, object argumentKeywords);

        IDisposable Subscribe(IWampTopicSubscriber subscriber, object options);
    }
}