using System;
using System.Threading.Tasks;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Client
{
    public interface IWampTopicProxy
    {
        string TopicUri { get; }

        Task<long> Publish(object options);
        Task<long> Publish(object options, object[] arguments);
        Task<long> Publish(object options, object[] arguments, object argumentKeywords);

        Task<IDisposable> Subscribe(IWampTopicSubscriber subscriber, object options);
    }
}