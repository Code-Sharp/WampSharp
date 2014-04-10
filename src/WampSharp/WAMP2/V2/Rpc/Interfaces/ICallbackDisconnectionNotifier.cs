using System;

namespace WampSharp.V2.Rpc
{
    public interface ICallbackDisconnectionNotifier
    {
        event EventHandler Disconnected;
    }
}