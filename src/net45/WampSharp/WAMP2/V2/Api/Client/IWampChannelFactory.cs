using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;

namespace WampSharp.V2
{
    /// <summary>
    /// Represents a factory for <see cref="IWampChannel"/>s.
    /// </summary>
    public interface IWampChannelFactory
    {
        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given connection and the given binding.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="connection">The connection to use to connect to the router.</param>
        /// <param name="binding">The binding to use to communicate with the router.</param>
        /// <returns></returns>
        IWampChannel CreateChannel<TMessage>
            (string realm,
             IControlledWampConnection<TMessage> connection,
             IWampBinding<TMessage> binding);

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given connection and the given binding.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="connection">The connection to use to connect to the router.</param>
        /// <param name="binding">The binding to use to communicate with the router.</param>
        /// <param name="authenticator">The authenticator object to handle CHALLENGE request.</param>
        /// <returns></returns>
        IWampChannel CreateChannel<TMessage>
            (string realm,
             IControlledWampConnection<TMessage> connection,
             IWampBinding<TMessage> binding,
             IWampClientAuthenticator authenticator);

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given connection factory and the given binding.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="connectionFactory">The factory to use to create a connection that will be used to connect to the router.</param>
        /// <param name="binding">The binding to use to communicate with the router.</param>
        /// <returns></returns>
        IWampChannel CreateChannel<TMessage>
            (string realm,
             Func<IControlledWampConnection<TMessage>> connectionFactory,
             IWampBinding<TMessage> binding);

        /// <summary>
        /// Creates a <see cref="IWampChannel"/> that connects to a given realm,
        /// using the given connection factory and the given binding.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="realm">The given realm to connect to.</param>
        /// <param name="connectionFactory">The factory to use to create a connection that will be used to connect to the router.</param>
        /// <param name="binding">The binding to use to communicate with the router.</param>
        /// <param name="authenticator">The authenticator object to handle CHALLENGE request.</param>
        /// <returns></returns>
        IWampChannel CreateChannel<TMessage>
            (string realm,
             Func<IControlledWampConnection<TMessage>> connectionFactory,
             IWampBinding<TMessage> binding,
             IWampClientAuthenticator authenticator);
    }
}