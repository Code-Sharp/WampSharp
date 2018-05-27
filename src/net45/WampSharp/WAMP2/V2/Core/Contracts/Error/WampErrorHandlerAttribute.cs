using System;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Indicates that the following method is a WAMP2 error handler of a given type..
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class WampErrorHandlerAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of <see cref="WampErrorHandlerAttribute"/>.
        /// </summary>
        /// <param name="messageType">The <see cref="WampMessageType"/>
        /// this error handler handles.</param>
        public WampErrorHandlerAttribute(WampMessageType messageType)
        {
            MessageType = messageType;
        }

        /// <summary>
        /// The request type this error handler handles. 
        /// </summary>
        public WampMessageType MessageType { get; }
    }
}