using System;
using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts
{
    /// <summary>
    /// Indicates a method handles raw <see cref="WampMessage{TMessage}"/>s.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class WampRawHandlerAttribute : Attribute
    {
    }
}