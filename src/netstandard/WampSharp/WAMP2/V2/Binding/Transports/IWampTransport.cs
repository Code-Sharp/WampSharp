using System;
using WampSharp.Core.Listener;

namespace WampSharp.V2.Binding.Transports
{
    /// <summary>
    /// Represents a WAMP transport - that is a mechanism that supplies a <see cref="IWampConnectionListener{TMessage}"/>
    /// given a <see cref="IWampBinding{TMessage}"/>.
    /// </summary>
    public interface IWampTransport : IDisposable
    {
        /// <summary>
        /// Opens this transport.
        /// </summary>
        void Open();

        /// <summary>
        /// Gets a <see cref="IWampConnectionListener{TMessage}"/> that is suitable
        /// for the given <see cref="IWampBinding{TMessage}"/>.
        /// </summary>
        /// <param name="binding">The given <see cref="IWampBinding{TMessage}"/>.</param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns>The requested <see cref="IWampConnectionListener{TMessage}"/>.</returns>
        IWampConnectionListener<TMessage> GetListener<TMessage>(IWampBinding<TMessage> binding);         
    }

    /// <summary>
    /// Represents a <see cref="IWampTransport"/> for a given underlying raw type.
    /// </summary>
    /// <typeparam name="TRaw"></typeparam>
    public interface IWampTransport<TRaw> : IWampTransport
    {
        /// <summary>
        /// Gets a <see cref="IWampConnectionListener{TMessage}"/> that is suitable
        /// for the given <see cref="IWampBinding{TMessage}"/>.
        /// </summary>
        /// <param name="binding">The given <see cref="IWampBinding{TMessage}"/>.</param>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns>The requested <see cref="IWampConnectionListener{TMessage}"/>.</returns>
        IWampConnectionListener<TMessage> GetListener<TMessage>(IWampTransportBinding<TMessage, TRaw> binding);
    }
}