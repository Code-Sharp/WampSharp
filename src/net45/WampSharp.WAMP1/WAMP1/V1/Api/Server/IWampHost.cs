using System;
using WampSharp.V1.PubSub.Server;
using WampSharp.V1.Core.Listener;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.V1
{
    public interface IWampHost : IDisposable
    {
        void Open();
        void HostService(object instance, string baseUri = null);

        IWampTopicContainer TopicContainer { get; }
        void Register(IWampRpcMetadata rpcMetadata);
        void Unregister(IWampRpcMethod method);
        event EventHandler<WampSessionEventArgs> SessionCreated;
        event EventHandler<WampSessionEventArgs> SessionClosed;
    }
}