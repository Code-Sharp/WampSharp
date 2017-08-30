﻿
using System;
using WampSharp.Binding;
using WampSharp.Core.Listener;
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
            Func<IControlledWampConnection<TMessage>> connectionFactory =
                () => new WebSocket4NetTextConnection<TMessage>(address, binding);

            return this.CreateChannel(realm, connectionFactory, binding);
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
            Func<IControlledWampConnection<TMessage>> connectionFactory =
                () => new WebSocket4NetTextConnection<TMessage>(address, binding);

            return this.CreateChannel(realm, connectionFactory, binding, authenticator);
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
            Func<IControlledWampConnection<TMessage>> connectionFactory =
                () => new WebSocket4NetBinaryConnection<TMessage>(address, binding);

            return this.CreateChannel(realm, connectionFactory, binding);
        }

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given address and the given binary binding
        /// </summary>
        /// <param name="address">The given address.</param>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="binding">The given binary binding.</param>
        /// <param name="authenticator">The authenticator object to handle CHALLENGE request.</param>
        /// <returns></returns>
        public IWampChannel CreateChannel<TMessage>(string address,
                                                    string realm,
                                                    IWampBinaryBinding<TMessage> binding,
                                                    IWampClientAuthenticator authenticator)
        {
            Func<IControlledWampConnection<TMessage>> connectionFactory =
                () => new WebSocket4NetBinaryConnection<TMessage>(address, binding);

            return this.CreateChannel(realm, connectionFactory, binding, authenticator);
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
        /// <param name="authenticator">The authenticator object to handle CHALLENGE request.</param>
        /// <returns></returns>
        public IWampChannel CreateMsgpackChannel(string address,
                                                 string realm,
                                                 IWampClientAuthenticator authenticator)
        {
            return this.CreateChannel(address, realm, mMsgpackBinding, authenticator);
        }
    }
}
