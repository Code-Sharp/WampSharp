using System.Reflection;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    /// <summary>
    /// Represents an interface that allows to get involved in callee registration.
    /// </summary>
    public interface ICalleeRegistrationInterceptor
    {
        /// <summary>
        /// Gets the options that will be used to register this method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        RegisterOptions GetOptions(MethodInfo method);

        /// <summary>
        /// Gets the procedure uri that will be used to register this method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        string GetProcedureUri(MethodInfo method);
    }
}