using System;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Core.Curie;
using WampSharp.V1.Cra;

namespace WampSharp.V1.Core.Listener.ClientBuilder
{
    internal class WampClientProxy : WampClientProxyBase, IWampClient, IDisposable, IWampCurieMapper, IWampConnectionMonitor
    {
        private readonly IWampCurieMapper mCurieMapper = new WampCurieMapper();
        private readonly IWampConnectionMonitor mMonitor;
        private readonly IDisposable mDisposable;

        public WampClientProxy(IWampOutgoingMessageHandler messageHandler,
                               IWampOutgoingRequestSerializer requestSerializer,
                               IWampConnectionMonitor monitor,
                               IDisposable disposable) :
            base(messageHandler, requestSerializer)
        {
            mMonitor = monitor;
            mDisposable = disposable;
        }

        public string SessionId { get; set; }

        public IWampCraAuthenticator CraAuthenticator { get; set; }

        public void Dispose()
        {
            mDisposable?.Dispose();
        }

        public string Resolve(string curie)
        {
            return mCurieMapper.Resolve(curie);
        }

        public void Map(string prefix, string uri)
        {
            mCurieMapper.Map(prefix, uri);
        }

        public event EventHandler ConnectionClosed
        {
            add => mMonitor.ConnectionClosed += value;
            remove => mMonitor.ConnectionClosed -= value;
        }

        public bool Connected => mMonitor.Connected;
    }
}