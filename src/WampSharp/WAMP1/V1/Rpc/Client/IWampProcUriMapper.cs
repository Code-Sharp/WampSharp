using System.Reflection;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// Maps WAMP RPC method calls to their corresponding
    /// uris.
    /// </summary>
    public interface IWampProcUriMapper
    {
        /// <summary>
        /// Maps the given method to its corresponding uri.
        /// </summary>
        /// <param name="method">The given method.</param>
        /// <returns>The method's corresponding uri.</returns>
        string Map(MethodInfo method);
    }
}