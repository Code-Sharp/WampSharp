using System;
using WampSharp.Core.Listener;

namespace WampSharp.V1.Auxiliary.Client
{
    public interface IWampClientConnectionMonitor
    {
        string SessionId { get; }

        void MapPrefix(string prefix, string uri);
        
        event EventHandler<WampConnectionEstablishedEventArgs> ConnectionEstablished;
        event EventHandler ConnectionLost;
        event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
    }
}