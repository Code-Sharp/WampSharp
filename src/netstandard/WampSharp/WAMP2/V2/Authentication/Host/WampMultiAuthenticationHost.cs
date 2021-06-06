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
    public class WampMultiAuthenticationHost : WampHost, IWampMultiAuthenticationHost
    {
        private readonly IWampSessionAuthenticatorFactory mDefaultAuthenticatorFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="WampMultiAuthenticationHost"/> given the
        /// <see cref="IWampSessionAuthenticatorFactory"/> to use.
        /// </summary>
        /// <param name="defaultAuthenticatorFactory">A default <see cref="IWampSessionAuthenticatorFactory"/> that will be used for the overload <see cref="RegisterTransport(IWampTransport,IEnumerable{WampSharp.V2.Binding.IWampBinding})"/></param>
        /// <param name="realmContainer">The <see cref="IWampRealmContainer"/> associated with this
        /// host.</param>
        /// <param name="uriValidator">The <see cref="IWampUriValidator"/> used to validate uris.</param>
        public WampMultiAuthenticationHost(IWampSessionAuthenticatorFactory defaultAuthenticatorFactory = null,
                                           IWampRealmContainer realmContainer = null,
                                           IWampUriValidator uriValidator = null)
            : base(realmContainer, uriValidator)
        {
            mDefaultAuthenticatorFactory = defaultAuthenticatorFactory;
        }

        public void RegisterTransport(IWampTransport transport, IDictionary<IWampBinding, IWampSessionAuthenticatorFactory> bindingToAuthenticatorFactory)
        {
            IEnumerable<IWampBinding> authenticationBindings =
                bindingToAuthenticatorFactory
                    .Select(binding =>
                                CreateAuthenticationBinding((dynamic) binding.Key,
                                                            binding.Value))
                    .Cast<IWampBinding>()
                    .Where(x => x != null);

            base.RegisterTransport(transport, authenticationBindings);
        }

        public override void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> bindings)
        {
            if (mDefaultAuthenticatorFactory == null)
            {
                ThrowHelper.NoDefaultSessionAuthenticatorWasProvided();
            }

            IDictionary<IWampBinding, IWampSessionAuthenticatorFactory> bindingToDefaultAuthenticator = 
                bindings.ToDictionary(x => x, x => mDefaultAuthenticatorFactory);

            RegisterTransport(transport, bindingToDefaultAuthenticator);
        }

        private IWampBinding CreateAuthenticationBinding<TMessage>
            (IWampTextBinding<TMessage> binding, IWampSessionAuthenticatorFactory authenticatorFactory)
        {
            ThrowHelper.ValidateSessionAuthenticatorFactoryWasProvided(authenticatorFactory, binding);
            return new WampAuthenticationTextBinding<TMessage>(binding, authenticatorFactory, this.UriValidator);
        }

        private IWampBinding CreateAuthenticationBinding<TMessage>
            (IWampBinaryBinding<TMessage> binding, IWampSessionAuthenticatorFactory authenticatorFactory)
        {
            ThrowHelper.ValidateSessionAuthenticatorFactoryWasProvided(authenticatorFactory, binding);
            return new WampAuthenticationBinaryBinding<TMessage>(binding, authenticatorFactory, this.UriValidator);
        }

        private IWampBinding CreateAuthenticationBinding<TMessage>
            (IWampBinding<TMessage> binding, IWampSessionAuthenticatorFactory authenticatorFactory)
        {
            ThrowHelper.ValidateSessionAuthenticatorFactoryWasProvided(authenticatorFactory, binding);
            return new WampAuthenticationBinding<TMessage>(binding, authenticatorFactory, this.UriValidator);
        }

        /// <summary>
        /// Fallback in case that binding doesn't implement
        /// IWampBinding{TMessage}
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="authenticatorFactory"></param>
        /// <returns></returns>
        private IWampBinding CreateAuthenticationBinding(IWampBinding binding, IWampSessionAuthenticatorFactory authenticatorFactory)
        {
            return null;
        }

        private static class ThrowHelper
        {
            public static void ValidateSessionAuthenticatorFactoryWasProvided(IWampSessionAuthenticatorFactory authenticatorFactory, IWampBinding binding)
            {
                if (authenticatorFactory == null)
                {
                    throw new
                        ArgumentException($"No IWampSessionAuthenticatorFactory was provided for the binding named '{binding.Name}'.");
                }
            }

            public static void NoDefaultSessionAuthenticatorWasProvided()
            {
                throw new
                    ArgumentException("No default IWampSessionAuthenticatorFactory was provided. Either provide a default IWampSessionAuthenticatorFactory in the constructor, or call RegisterTransport with the desired IWampSessionAuthenticatorFactory");
            }
        }
    }
}