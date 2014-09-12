using System;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class WampDetailsOptionsAttribute : Attribute
    {
        private readonly WampMessageType mMessageType;

        public WampDetailsOptionsAttribute(WampMessageType messageType)
        {
            mMessageType = messageType;
        }

        public WampMessageType MessageType
        {
            get { return mMessageType; }
        }
    }
}