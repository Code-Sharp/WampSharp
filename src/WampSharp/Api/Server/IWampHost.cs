using System;
using WampSharp.Core.Listener.V1;
using WampSharp.PubSub.Server;
using WampSharp.Rpc.Server;

namespace WampSharp
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