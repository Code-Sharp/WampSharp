using System;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Client
{
    internal interface IWampTopicSubscriptionProxy
    {
        Task<IDisposable> Subscribe(IWampRawTopicSubscriber subscriber, SubscribeOptions options, string topicUri);
    }
}