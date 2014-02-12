using System;
using WampSharp.V1.PubSub.Server.Interfaces;

namespace WampSharp.V1.Api.Server
{
    public interface IWampHost : IDisposable
    {
        void Open();
        void HostService(object instance, string baseUri = null);

        IWampTopicContainer TopicContainer { get; }
    }
}