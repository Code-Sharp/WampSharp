using System;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Represents error <see cref="EventArgs"/>.
    /// </summary>
    /// <remarks>
    /// Used for <see cref="IWampConnection{TMessage}.ConnectionError"/>;
    /// </remarks>
    public class WampConnectionErrorEventArgs : EventArgs
    {

        /// <summary>
        /// Initializes an new instance of <see cref="WampConnectionErrorEventArgs"/>.
        /// </summary>
        /// <param name="exception">The exception that describes this error.</param>
        public WampConnectionErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }

        /// <summary>
        /// Gets the exception that represents the error.
        /// </summary>
        public Exception Exception { get; }
    }
}