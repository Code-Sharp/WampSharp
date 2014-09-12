using System;

namespace WampSharp.V2.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class IgnorePropertyAttribute : Attribute
    {
    }
}