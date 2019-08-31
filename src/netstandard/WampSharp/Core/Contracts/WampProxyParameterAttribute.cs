using System;

namespace WampSharp.Core.Contracts
{
    /// <summary>
    /// Indicates that a given parameter of a <see cref="WampHandlerAttribute"/> method 
    /// is a proxy to a WAMP contract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class WampProxyParameterAttribute : Attribute
    {
    }
}