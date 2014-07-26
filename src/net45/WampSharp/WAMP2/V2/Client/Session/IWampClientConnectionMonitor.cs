using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Client
{
    public interface IWampClientConnectionMonitor
    {
        event EventHandler<WampSessionEventArgs> ConnectionEstablished;
        event EventHandler<WampSessionCloseEventArgs> ConnectionBroken;
        event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
    }
}