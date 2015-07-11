#if PCL
using System;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm.Binded;

namespace WampSharp.V2.Core.Listener.ClientBuilder
{
    internal class WampClientProxy<TMessage> : WampClientProxyBase, 
        IWampClientProxy<TMessage>,
        IWampConnectionMonitor,
        IDisposable
    {
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

        public bool GoodbyeSent { get; set; }
        
        public long Session { get; set; }

        public IWampBindedRealm<TMessage> Realm { get; set; }
        
        public IWampBinding<TMessage> Binding { get; set; }

        public IWampSessionAuthenticator Authenticator { get; set; }

        public IWampAuthorizer Authorizer
        {
            get
            {
                return Authenticator.Authorizer;
            }
        }

        public ClientRoles Roles { get; set; }

        IWampBinding IWampClientProperties.Binding
        {
            get { return Binding; }
        }

        public void Message(WampMessage<object> message)
        {
            Send(message);
        }

        public event EventHandler ConnectionClosed
        {
            add
            {
                mMonitor.ConnectionClosed += value;
            }
            remove
            {
                mMonitor.ConnectionClosed -= value;
            }
        }

        public bool Connected
        {
            get
            {
                return mMonitor.Connected;
            }
        }

        public void Dispose()
        {
            mDisposable.Dispose();
        }
    }
}
#endif