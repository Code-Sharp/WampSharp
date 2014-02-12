using System;

namespace WampSharp.V1.Auxiliary.Client
{
    public interface IWampClientConnectionMonitor
    {
        void MapPrefix(string prefix, string uri);
        
        event EventHandler<WampConnectionEstablishedEventArgs> ConnectionEstablished;
        event EventHandler ConnectionLost;
    }
}