using System;

namespace WampSharp.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class WampRawHandlerAttribute : Attribute
    {
    }
}