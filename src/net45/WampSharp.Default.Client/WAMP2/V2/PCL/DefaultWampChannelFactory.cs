#if PCL
using WampSharp.Binding;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.Windows;

namespace WampSharp.V2
{
    /// <summary>
    /// A default implementation of <see cref="IWampChannelFactory"/>.
    /// </summary>
    public class DefaultWampChannelFactory : WampChannelFactory
    {
        private readonly JTokenJsonBinding mJsonBinding = new JTokenJsonBinding();

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given address and the given text binding
        /// </summary>
        /// <param name="address">The given address.</param>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="binding">The given text binding.</param>
        /// <returns></returns>
        public IWampChannel CreateChannel<TMessage>(string address,
                                                    string realm,
                                                    IWampTextBinding<TMessage> binding)
        {
            var connection =
                new MessageWebSocketTextConnection<TMessage>(address, binding);

            return this.CreateChannel(realm, connection, binding);
        }

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given address and the given text binding
        /// </summary>
        /// <param name="address">The given address.</param>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="binding">The given text binding.</param>
        /// <param name="authenticator">The authenticator object to handle CHALLENGE request.</param>
        /// <returns></returns>
        public IWampChannel CreateChannel<TMessage>(string address,
                                                    string realm,
                                                    IWampTextBinding<TMessage> binding,
                                                    IWampClientAuthenticator authenticator)
        {
            var connection =
                new MessageWebSocketTextConnection<TMessage>(address, binding);

            return this.CreateChannel(realm, connection, binding, authenticator);
        }

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given address and json binding
        /// </summary>
        /// <param name="address">The given address.</param>
        /// <param name="realm">The given realm to connect to.</param>
        /// <returns></returns>
        public IWampChannel CreateJsonChannel(string address,
                                              string realm)
        {
            return this.CreateChannel(address, realm, mJsonBinding);
        }

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given address and json binding
        /// </summary>
        /// <param name="address">The given address.</param>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="authenticator">The authenticator object to handle CHALLENGE request.</param>
        /// <returns></returns>
        public IWampChannel CreateJsonChannel(string address,
                                              string realm,
                                              IWampClientAuthenticator authenticator)
        {
            return this.CreateChannel(address, realm, mJsonBinding, authenticator);
        }
    }
}
#endif