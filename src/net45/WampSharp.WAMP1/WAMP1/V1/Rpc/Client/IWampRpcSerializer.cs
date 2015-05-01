using System.Reflection;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// Serializes proxy method calls to <see cref="WampRpcCall"/>s.
    /// </summary>
    public interface IWampRpcSerializer
    {
        /// <summary>
        /// Serializes a proxy method call to a <see cref="WampRpcCall"/>.
        /// </summary>
        /// <param name="method">The called method.</param>
        /// <param name="arguments">The call arguments.</param>
        /// <returns>The serialized <see cref="WampRpcCall"/>.</returns>
        WampRpcCall Serialize(MethodInfo method, object[] arguments);
    }
}