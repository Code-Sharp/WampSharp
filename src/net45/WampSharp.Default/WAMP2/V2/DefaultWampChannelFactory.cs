using WampSharp.Binding;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.WebSocket4Net;

namespace WampSharp.V2
{
    /// <summary>
    /// A default implementation of <see cref="IWampChannelFactory"/>.
    /// </summary>
    public class DefaultWampChannelFactory : WampChannelFactory
    {
        private readonly JTokenMsgpackBinding mMsgpackBinding = new JTokenMsgpackBinding();
        private readonly JTokenJsonBinding mJsonBinding = new JTokenJsonBinding();
        private readonly IWampClientAutenticator mDefaultAutenticator = new DefaultWampClientAutenticator();

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
                new WebSocket4NetTextConnection<TMessage>(address, binding);

            return this.CreateChannel(realm, connection, binding, mDefaultAutenticator);
        }

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given address and the given text binding
        /// </summary>
        /// <param name="address">The given address.</param>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="binding">The given text binding.</param>
        /// <param name="autenticator">The authenticator object to handle CHALLENGE request.</param>
        /// <returns></returns>
        public IWampChannel CreateChannel<TMessage>(string address,
                                                    string realm,
                                                    IWampTextBinding<TMessage> binding,
                                                    IWampClientAutenticator autenticator)
        {
            var connection =
                new WebSocket4NetTextConnection<TMessage>(address, binding);

            return this.CreateChannel(realm, connection, binding, autenticator);
        }

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given address and the given binary binding
        /// </summary>
        /// <param name="address">The given address.</param>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="binding">The given binary binding.</param>
        /// <returns></returns>
        public IWampChannel CreateChannel<TMessage>(string address,
                                                    string realm,
                                                    IWampBinaryBinding<TMessage> binding)
        {
            var connection =
                new WebSocket4NetBinaryConnection<TMessage>(address, binding);

            return this.CreateChannel(realm, connection, binding, mDefaultAutenticator);
        }

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given address and the given binary binding
        /// </summary>
        /// <param name="address">The given address.</param>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="binding">The given binary binding.</param>
        /// <param name="autenticator">The authenticator object to handle CHALLENGE request.</param>
        /// <returns></returns>
        public IWampChannel CreateChannel<TMessage>(string address,
                                                    string realm,
                                                    IWampBinaryBinding<TMessage> binding,
                                                    IWampClientAutenticator autenticator)
        {
            var connection =
                new WebSocket4NetBinaryConnection<TMessage>(address, binding);

            return this.CreateChannel(realm, connection, binding, autenticator);
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
        /// <param name="autenticator">The authenticator object to handle CHALLENGE request.</param>
        /// <returns></returns>
        public IWampChannel CreateJsonChannel(string address,
                                              string realm,
                                              IWampClientAutenticator autenticator)
        {
            return this.CreateChannel(address, realm, mJsonBinding, autenticator);
        }

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given address and msgpack binding
        /// </summary>
        /// <param name="address">The given address.</param>
        /// <param name="realm">The given realm to connect to.</param>
        /// <returns></returns>
        public IWampChannel CreateMsgpackChannel(string address,
                                                 string realm)
        {
            return this.CreateChannel(address, realm, mMsgpackBinding);
        }

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given address and msgpack binding
        /// </summary>
        /// <param name="address">The given address.</param>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="autenticator">The authenticator object to handle CHALLENGE request.</param>
        /// <returns></returns>
        public IWampChannel CreateMsgpackChannel(string address,
                                                 string realm,
                                                 IWampClientAutenticator autenticator)
        {
            return this.CreateChannel(address, realm, mMsgpackBinding, autenticator);
        }
    }
}