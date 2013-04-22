using System;
using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class WampHandlerAttribute : Attribute
    {
        private readonly WampMessageType mMessageType;

        public WampHandlerAttribute(WampMessageType messageType)
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
