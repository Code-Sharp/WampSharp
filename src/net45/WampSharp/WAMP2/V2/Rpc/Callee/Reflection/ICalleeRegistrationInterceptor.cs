using System;
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
        /// Returns a value indicating whether this method is a callee method. Deprecated. Use <see cref="IsCalleeMember"/>
        /// </summary>
#if !PCL
        [Obsolete("Deprecated.  Use IsCalleeMember for readonly property support")]
#endif
        bool IsCalleeProcedure(MethodInfo method);
        
        /// <summary>
        /// Returns a value indicating whether this method is a callee method/member.
        /// </summary>
        bool IsCalleeMember(MemberInfo member);
        
        /// <summary>
        /// Gets the options that will be used to register given memember
        /// </summary>
        RegisterOptions GetRegisterOptions(MemberInfo member);

        /// <summary>
        /// Gets the procedure uri that will be used to register the given method.
        /// </summary>
        string GetProcedureUri(MemberInfo member);

    }
}