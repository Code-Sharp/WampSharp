namespace WampSharp.V2.Core
{
    /// <summary>
    /// Generates request and session ids.
    /// </summary>
    public interface IWampIdGenerator
    {
        /// <summary>
        /// Generates a id.
        /// </summary>
        /// <returns>The generated id.</returns>
        long Generate();
    }
}