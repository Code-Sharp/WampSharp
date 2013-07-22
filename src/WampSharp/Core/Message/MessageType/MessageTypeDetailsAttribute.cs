using System;

namespace WampSharp.Core.Message
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class MessageTypeDetailsAttribute : Attribute
    {
        private readonly MessageDirection mDirection;
        private readonly MessageCategory mCategory;
        private readonly int mProtocolVersion;

        public MessageTypeDetailsAttribute(MessageDirection direction, MessageCategory category, int protocolVersion)
        {
            mDirection = direction;
            mCategory = category;
            mProtocolVersion = protocolVersion;
        }

        public MessageDirection Direction
        {
            get { return mDirection; }
        }

        public MessageCategory Category
        {
            get { return mCategory; }
        }

        public int ProtocolVersion
        {
            get { return mProtocolVersion; }
        }
    }
}