using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// Contains events of connection status.
    /// </summary>
    public interface IWampClientConnectionMonitor
    {
        /// <summary>
        /// Occurs when a connection is established.
        /// </summary>
        event EventHandler<WampSessionCreatedEventArgs> ConnectionEstablished;

        /// <summary>
        /// Occurs when a connection is broken.
        /// </summary>
        event EventHandler<WampSessionCloseEventArgs> ConnectionBroken;

        /// <summary>
        /// Occurs when a connection is faulted.
        /// </summary>
        event EventHandler<WampConnectionErrorEventArgs> ConnectionError;

        /// <summary>
        /// Gets a value indicating whether the channel is currently connected.
        /// </summary>
        bool IsConnected { get; }
    }
}