namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Represents a <see cref="IWampConnection{TMessage}"/> that its state
    /// can be controlled.
    /// </summary>
    /// <remarks>
    /// This interface was created in order to apply client side connection
    /// capabilites.
    /// </remarks>
    /// <typeparam name="TMessage"></typeparam>
    public interface IControlledWampConnection<TMessage> : 
        IWampConnection<TMessage>
    {
        /// <summary>
        /// Tries to establish a connection to the remote server.
        /// </summary>
        void Connect();
    }
}