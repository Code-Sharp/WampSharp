using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using WampSharp.Binding;
using WampSharp.Fleck;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Core;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    /// <summary>
    /// A default implementation of <see cref="WampAuthenticationHost"/> that uses
    /// <see cref="FleckWebSocketTransport"/> internally.
    /// </summary>
    public class DefaultWampAuthenticationHost : WampAuthenticationHost
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampAuthenticationHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="sessionAuthenticationFactory">The <see cref="IWampSessionAuthenticatorFactory"/>
        /// used to accept pending clients.</param>
        /// <param name="supportDualStack">IPv4/IPv6 dual stack support</param>
        public DefaultWampAuthenticationHost
        (string location,
            IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
            : this(location: location,
                sessionAuthenticationFactory: sessionAuthenticationFactory,
                supportDualStack: true,
                cookieAuthenticatorFactory: null,
                certificate: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampAuthenticationHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="sessionAuthenticationFactory">The <see cref="IWampSessionAuthenticatorFactory"/>
        /// used to accept pending clients.</param>
        /// <param name="supportDualStack">IPv4/IPv6 dual stack support</param>
        public DefaultWampAuthenticationHost
            (string location,
             IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
             bool supportDualStack = true)
            : this(location: location,
                   sessionAuthenticationFactory: sessionAuthenticationFactory,
                   supportDualStack: supportDualStack,
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
        /// <param name="sessionAuthenticationFactory">The <see cref="IWampSessionAuthenticatorFactory"/>
        /// used to accept pending clients.</param>
        /// <param name="cookieAuthenticatorFactory">The given <see cref="ICookieAuthenticatorFactory"/> used to authenticate
        /// users given their cookies.</param>
        /// <param name="certificate">The <see cref="X509Certificate2"/> certificate to use for secured websockets.</param>
        /// <param name="supportDualStack">IPv4/IPv6 dual stack support</param>
        public DefaultWampAuthenticationHost
            (string location,
             IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
             ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
             X509Certificate2 certificate = null,
             bool supportDualStack = true)
            : this(location: location,
                   sessionAuthenticationFactory: sessionAuthenticationFactory,
                   bindings: null,
                   cookieAuthenticatorFactory: cookieAuthenticatorFactory,
                   certificate: certificate,
                   supportDualStack: supportDualStack)
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
        /// <param name="certificate">The <see cref="X509Certificate2"/> certificate to use for secured websockets.</param>
        /// <param name="supportDualStack">IPv4/IPv6 dual stack support</param>
        public DefaultWampAuthenticationHost
            (string location,
             IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
             IEnumerable<IWampBinding> bindings = null,
             ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
             X509Certificate2 certificate = null,
             bool supportDualStack = true)
            : this(location: location,
                   sessionAuthenticationFactory: sessionAuthenticationFactory,
                   realmContainer: null,
                   uriValidator: null,
                   bindings: bindings,
                   cookieAuthenticatorFactory: cookieAuthenticatorFactory,
                   certificate: certificate,
                   supportDualStack: supportDualStack)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="sessionAuthenticationFactory">The <see cref="IWampSessionAuthenticatorFactory"/>
        /// used to accept pending clients.</param>
        /// <param name="realmContainer">The <see cref="IWampRealmContainer"/> associated with this
        /// host.</param>
        /// <param name="uriValidator">The <see cref="IWampUriValidator"/> used to validate uris.</param>
        /// <param name="bindings">The given bindings.</param>
        /// <param name="cookieAuthenticatorFactory">The given <see cref="ICookieAuthenticatorFactory"/> used to authenticate
        /// users given their cookies.</param>
        /// <param name="certificate">The <see cref="X509Certificate2"/> certificate to use for secured websockets.</param>
        /// <param name="supportDualStack">IPv4/IPv6 dual stack support</param>
        public DefaultWampAuthenticationHost(string location,
            IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
            IWampRealmContainer realmContainer = null,
            IWampUriValidator uriValidator = null,
            IEnumerable<IWampBinding> bindings = null,
            ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
            X509Certificate2 certificate = null,
            bool supportDualStack = true)
            : this(location: location,
                   sessionAuthenticationFactory: sessionAuthenticationFactory,
                   realmContainer: null,
                   uriValidator: null,
                   bindings: bindings,
                   cookieAuthenticatorFactory: cookieAuthenticatorFactory, 
                   certificate: certificate,
                   getEnabledSslProtocols: null,
                   supportDualStack: supportDualStack)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultWampHost"/> listening at
        /// the given location with the given bindings and the given
        /// <see cref="IWampRealmContainer"/>.
        /// </summary>
        /// <param name="location">The given location.</param>
        /// <param name="sessionAuthenticationFactory">The <see cref="IWampSessionAuthenticatorFactory"/>
        /// used to accept pending clients.</param>
        /// <param name="realmContainer">The <see cref="IWampRealmContainer"/> associated with this
        /// host.</param>
        /// <param name="uriValidator">The <see cref="IWampUriValidator"/> used to validate uris.</param>
        /// <param name="bindings">The given bindings.</param>
        /// <param name="cookieAuthenticatorFactory">The given <see cref="ICookieAuthenticatorFactory"/> used to authenticate
        /// users given their cookies.</param>
        /// <param name="certificate">The <see cref="X509Certificate2"/> certificate to use for secured websockets.</param>
        /// <param name="supportDualStack">IPv4/IPv6 dual stack support</param>
        /// <param name="getEnabledSslProtocols"> If non-null, used to set Fleck's EnabledSslProtocols. </param>
        public DefaultWampAuthenticationHost(string location,
                                             IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
                                             IWampRealmContainer realmContainer = null,
                                             IWampUriValidator uriValidator = null,
                                             IEnumerable<IWampBinding> bindings = null,
                                             ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
                                             X509Certificate2 certificate = null,
                                             bool supportDualStack = true,
                                             Func<SslProtocols> getEnabledSslProtocols = null)
            : base(sessionAuthenticationFactory, realmContainer, uriValidator)
        {
            bindings = bindings ?? new IWampBinding[] { new JTokenJsonBinding(), new JTokenMsgpackBinding() };

            this.RegisterTransport(
                new FleckAuthenticatedWebSocketTransport(location, cookieAuthenticatorFactory, certificate, getEnabledSslProtocols, supportDualStack),
                bindings.ToArray());
        }

        public override sealed void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> bindings)
        {
            base.RegisterTransport(transport, bindings);
        }
    }
}