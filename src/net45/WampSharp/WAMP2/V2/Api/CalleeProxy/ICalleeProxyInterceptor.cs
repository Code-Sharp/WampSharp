using System.Reflection;
using WampSharp.V2.Core.Contracts;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2
{
    /// <summary>
    /// Represents an interface that allows to get involved in callee proxy call.
    /// </summary>
    public interface ICalleeProxyInterceptor
    {
        /// <summary>
        /// Gets the call options for a given method.
        /// </summary>
        /// <param name="method">The given method.</param>
        /// <returns>The call options for the given method.</returns>
        CallOptions GetCallOptions(MethodInfo method);

        /// <summary>
        /// Gets the procedure uri for a given method.
        /// </summary>
        /// <param name="method">The given method.</param>
        /// <returns>The procedure uri for the given method.</returns>
        string GetProcedureUri(MethodInfo method);
    }
}