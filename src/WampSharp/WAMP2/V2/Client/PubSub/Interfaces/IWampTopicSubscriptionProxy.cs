using System;
using System.Threading.Tasks;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Client
{
    internal interface IWampTopicSubscriptionProxy
    {
        Task<IDisposable> Subscribe(IWampRawTopicSubscriber subscriber, object options, string topicUri);
    }
}