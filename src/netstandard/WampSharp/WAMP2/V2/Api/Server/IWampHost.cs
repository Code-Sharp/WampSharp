using System;
using System.Collections.Generic;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    /// <summary>
    /// Represents a WAMP host.
    /// </summary>
    public interface IWampHost : IDisposable
    {
        /// <summary>
        /// Gets a realm container associated with this host.
        /// </summary>
        IWampHostedRealmContainer RealmContainer { get; }

        /// <summary>
        /// Registers a given transport for this host.
        /// </summary>
        /// <param name="transport">The given transport to register.</param>
        /// <param name="bindings">The given bindings to activate support with the given transport.</param>
        void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> bindings);

        /// <summary>
        /// Opens this host.
        /// </summary>
        void Open();
    }
}