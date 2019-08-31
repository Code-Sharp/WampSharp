using System.Reflection;
using WampSharp.V2.Core.Contracts;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2
{
    /// <summary>
    /// Represents an interface that allows to get involved in callee registration.
    /// </summary>
    public interface ICalleeRegistrationInterceptor
    {
        /// <summary>
        /// Returns a value indicating whether this method is a callee method.
        /// </summary>
        bool IsCalleeProcedure(MethodInfo method);

        /// <summary>
        /// Gets the options that will be used to register the given method.
        /// </summary>
        RegisterOptions GetRegisterOptions(MethodInfo method);

        /// <summary>
        /// Gets the procedure uri that will be used to register the given method.
        /// </summary>
        string GetProcedureUri(MethodInfo method);
    }
}