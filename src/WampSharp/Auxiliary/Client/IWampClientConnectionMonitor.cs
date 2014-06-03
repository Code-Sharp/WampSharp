using System;

namespace WampSharp.Auxiliary.Client
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