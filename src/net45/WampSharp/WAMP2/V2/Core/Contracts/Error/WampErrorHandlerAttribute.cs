using System;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class WampErrorHandlerAttribute : Attribute
    {
        private readonly WampMessageType mMessageType;

        public WampErrorHandlerAttribute(WampMessageType messageType)
        {
            mMessageType = messageType;
        }

        public WampMessageType MessageType
        {
            get
            {
                return mMessageType;
            }
        }
    }
}