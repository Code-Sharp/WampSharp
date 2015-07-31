using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using WampSharp.Binding;
using WampSharp.Fleck;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    public class DefaultWampAuthenticatedHost : WampAuthenticationHost
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="sessionAuthenticationFactory"></param>
        public DefaultWampAuthenticatedHost
            (string location,
             IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
            : this(location: location,
                   sessionAuthenticationFactory: sessionAuthenticationFactory,
                   cookieAuthenticatorFactory: null,
                   certificate: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="sessionAuthenticationFactory"></param>
        /// <param name="cookieAuthenticatorFactory"></param>
        /// <param name="certificate"></param>
        public DefaultWampAuthenticatedHost
            (string location,
             IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
             ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
             X509Certificate2 certificate = null)
            : this(location: location,
                   sessionAuthenticationFactory: sessionAuthenticationFactory,
                   bindings: null,
                   cookieAuthenticatorFactory: cookieAuthenticatorFactory,
                   certificate: certificate)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="sessionAuthenticationFactory"></param>
        /// <param name="bindings">The given bindings.</param>
        /// <param name="cookieAuthenticatorFactory"></param>
        /// <param name="certificate"></param>
        public DefaultWampAuthenticatedHost
            (string location,
             IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
             IEnumerable<IWampBinding> bindings = null,
             ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
             X509Certificate2 certificate = null)
            : this(location: location,
                   sessionAuthenticationFactory: sessionAuthenticationFactory,
                   realmContainer: null,
                   bindings: bindings,
                   cookieAuthenticatorFactory: cookieAuthenticatorFactory,
                   certificate: certificate)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="sessionAuthenticationFactory"></param>
        /// <param name="realmContainer">The given <see cref="IWampRealmContainer"/>.</param>
        /// <param name="bindings">The given bindings.</param>
        /// <param name="cookieAuthenticatorFactory"></param>
        /// <param name="certificate"></param>
        public DefaultWampAuthenticatedHost(string location, 
            IWampSessionAuthenticatorFactory sessionAuthenticationFactory, 
            IWampRealmContainer realmContainer = null, 
            IEnumerable<IWampBinding> bindings = null, 
            ICookieAuthenticatorFactory cookieAuthenticatorFactory = null, 
            X509Certificate2 certificate = null)
            : base(sessionAuthenticationFactory, realmContainer)
        {
            bindings = bindings ?? new IWampBinding[] {new JTokenJsonBinding(), new JTokenMsgpackBinding()};
            
            this.RegisterTransport(new FleckAuthenticatedWebSocketTransport(location, cookieAuthenticatorFactory, certificate),
                                   bindings.ToArray());
        }

        public override sealed void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> bindings)
        {
            base.RegisterTransport(transport, bindings);
        }
    }
}