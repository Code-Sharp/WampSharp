using System;

namespace WampSharp.V2.Rpc
{
    /// <summary>
    /// Indicates that a method returns a progressive result.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WampProgressiveResultProcedureAttribute : Attribute
    {
    }
}