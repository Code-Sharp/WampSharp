using System;

namespace WampSharp.Core.Message
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class MessageTypeDetailsAttribute : Attribute
    {
        private readonly int mProtocolVersion;

        public MessageTypeDetailsAttribute(MessageDirection direction, MessageCategory category, int protocolVersion)
        {
            Direction = direction;
            Category = category;
            mProtocolVersion = protocolVersion;
        }

        public MessageDirection Direction { get; }

        public MessageCategory Category { get; }

        public int ProtocolVersion => mProtocolVersion;
    }
}