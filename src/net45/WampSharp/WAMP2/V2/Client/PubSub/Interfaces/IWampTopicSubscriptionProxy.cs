using System;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Client
{
    internal interface IWampTopicSubscriptionProxy
    {
        Task<IAsyncDisposable> Subscribe(IWampRawTopicClientSubscriber subscriber, SubscribeOptions options, string topicUri);
    }
}