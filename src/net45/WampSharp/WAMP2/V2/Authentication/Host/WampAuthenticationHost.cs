using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Core;
using WampSharp.V2.Realm;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2
{
    /// <summary>
    /// An implementation of <see cref="IWampHost"/> that supports authentication.
    /// </summary>
    public class WampAuthenticationHost : WampHost
    {
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="WampAuthenticationHost"/> given the
        /// <see cref="IWampSessionAuthenticatorFactory"/> to use.
        /// </summary>
        /// <param name="sessionAuthenticationFactory">The <see cref="IWampSessionAuthenticatorFactory"/>
        /// used to accept pending clients.</param>
        /// <param name="realmContainer">The <see cref="IWampRealmContainer"/> associated with this
        /// host.</param>
        /// <param name="uriValidator">The <see cref="IWampUriValidator"/> used to validate uris.</param>
        public WampAuthenticationHost(IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
            IWampRealmContainer realmContainer = null,
            IWampUriValidator uriValidator = null)
            : base(realmContainer, uriValidator)
        {
            if (sessionAuthenticationFactory == null)
            {
                throw new ArgumentNullException(nameof(sessionAuthenticationFactory));
            }

            mSessionAuthenticationFactory = sessionAuthenticationFactory;
        }

        public override void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> bindings)
        {
            IEnumerable<IWampBinding> authenticationBindings =
                bindings.Select(binding => CreateAuthenticationBinding((dynamic) binding))
                        .Cast<IWampBinding>()
                        .Where(x => x != null);

            base.RegisterTransport(transport, authenticationBindings);
        }

        private IWampBinding CreateAuthenticationBinding<TMessage>
            (IWampTextBinding<TMessage> binding)
        {
            return new WampAuthenticationTextBinding<TMessage>(binding, mSessionAuthenticationFactory, this.UriValidator);
        }

        private IWampBinding CreateAuthenticationBinding<TMessage>
            (IWampBinaryBinding<TMessage> binding)
        {
            return new WampAuthenticationBinaryBinding<TMessage>(binding, mSessionAuthenticationFactory, this.UriValidator);
        }

        private IWampBinding CreateAuthenticationBinding<TMessage>
            (IWampBinding<TMessage> binding)
        {
            return new WampAuthenticationBinding<TMessage>(binding, mSessionAuthenticationFactory, this.UriValidator);
        }

        /// <summary>
        /// Fallback in case that binding doesn't implement
        /// IWampBinding{TMessage}
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        private IWampBinding CreateAuthenticationBinding(IWampBinding binding)
        {
            return null;
        }
    }
}