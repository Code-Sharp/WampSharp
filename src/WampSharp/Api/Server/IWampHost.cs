using System;
using WampSharp.PubSub.Server;

namespace WampSharp
{
    public interface IWampHost : IDisposable
    {
        void Open();
        void HostService(object instance, string baseUri = null);

        IWampTopicContainer TopicContainer { get; }
    }
}