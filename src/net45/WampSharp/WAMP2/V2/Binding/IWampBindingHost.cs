using System;

namespace WampSharp.V2.Binding
{
    /// <summary>
    /// Represents a host for a specific <see cref="IWampBinding{TMessage}"/>.
    /// </summary>
    /// TODO: Get rid of this?
    public interface IWampBindingHost : IDisposable
    {
        /// <summary>
        /// Opens the host, so it starts listening for messages.
        /// </summary>
        void Open();
    }
}