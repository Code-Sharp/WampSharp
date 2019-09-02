using System;
using WampSharp.Core.Proxy;

namespace WampSharp.V1.Core.Listener.ClientBuilder
{
    internal class WampServerProxy : WampServerProxyBase, IDisposable
    {
        private readonly IDisposable mDisposable;

        public WampServerProxy(IWampOutgoingMessageHandler messageHandler, 
                               IWampOutgoingRequestSerializer requestSerializer,
                               IDisposable disposable) : 
            base(messageHandler, requestSerializer)
        {
            mDisposable = disposable;
        }

        public void Dispose()
        {
            mDisposable.Dispose();
        }
    }
}