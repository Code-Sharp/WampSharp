using System;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Allows getting notified when a WAMP client gets disconnected.
    /// </summary>
    public interface IWampConnectionMonitor
    {
        /// <summary>
        /// Occurs when a connection is closed.
        /// </summary>
        event EventHandler ConnectionClosed;
    }
}