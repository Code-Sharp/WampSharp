namespace WampSharp.V1.Core.Listener
{
    /// <summary>
    /// Generates session ids for clients.
    /// </summary>
    public interface IWampSessionIdGenerator
    {
        /// <summary>
        /// Generates a session id for a client.
        /// </summary>
        /// <returns>The generated session id.</returns>
        string Generate();
    }
}