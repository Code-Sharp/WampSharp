using System;

namespace WampSharp.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class WampProxyParameterAttribute : Attribute
    {
    }
}