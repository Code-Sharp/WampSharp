namespace WampSharp.V2.Core.Listener
{
    public interface IWampIdGenerator
    {
        /// <summary>
        /// Generates a session id for a client.
        /// </summary>
        /// <returns>The generated session id.</returns>
        long Generate();
    }
}