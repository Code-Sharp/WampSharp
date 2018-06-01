using System;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class WampDetailsOptionsAttribute : Attribute
    {
        public WampDetailsOptionsAttribute(WampMessageType messageType)
        {
            MessageType = messageType;
        }

        public WampMessageType MessageType { get; }
    }
}