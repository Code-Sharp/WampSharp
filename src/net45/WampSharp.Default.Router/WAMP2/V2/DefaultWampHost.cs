using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using WampSharp.Binding;
using WampSharp.Fleck;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    /// <summary>
    /// A default implementation of <see cref="WampHost"/> that uses
    /// <see cref="FleckWebSocketTransport"/> internally.
    /// </summary>
    public class DefaultWampHost : WampHost
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        public DefaultWampHost(string location)
            : this(location: location, certificate: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="certificate"></param>
        public DefaultWampHost(string location, X509Certificate2 certificate = null)
            : this(location: location, bindings: null, certificate: certificate)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="bindings">The given bindings.</param>
        /// <param name="certificate"></param>
        public DefaultWampHost(string location, IEnumerable<IWampBinding> bindings, X509Certificate2 certificate = null)
            : this(location: location, realmContainer: null, bindings: bindings, certificate: certificate)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="realmContainer">The given <see cref="IWampRealmContainer"/>.</param>
        /// <param name="bindings">The given bindings.</param>
        /// <param name="certificate"></param>
        public DefaultWampHost(string location, IWampRealmContainer realmContainer = null, IEnumerable<IWampBinding> bindings = null, X509Certificate2 certificate = null)
            : base(realmContainer)
        {
            bindings = bindings ?? new IWampBinding[] {new JTokenJsonBinding(), new JTokenMsgpackBinding()};

            this.RegisterTransport(new FleckWebSocketTransport(location, certificate),
                                   bindings.ToArray());
        }

        public sealed override void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> bindings)
        {
            base.RegisterTransport(transport, bindings);
        }
    }
}