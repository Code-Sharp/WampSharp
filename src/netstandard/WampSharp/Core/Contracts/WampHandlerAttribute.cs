using System;
using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts
{
    /// <summary>
    /// Indicates that a method treats a specific <see cref="WampMessageType"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class WampHandlerAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of <see cref="WampHandlerAttribute"/>
        /// with the given <see cref="MessageType"/>.
        /// </summary>
        /// <param name="messageType">The <see cref="WampMessageType"/> this
        /// method handles</param>
        public WampHandlerAttribute(WampMessageType messageType)
        {
            MessageType = messageType;
        }

        /// <summary>
        /// Gets the <see cref="WampMessageType"/> this method handles.
        /// </summary>
        public WampMessageType MessageType { get; }
    }
}
