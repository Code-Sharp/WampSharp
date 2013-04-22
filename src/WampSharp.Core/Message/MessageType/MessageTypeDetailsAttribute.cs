using System;

namespace WampSharp.Core.Message
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class MessageTypeDetailsAttribute : Attribute
    {
        private readonly MessageDirection mDirection;
        private readonly MessageCategory mCategory;

        public MessageTypeDetailsAttribute(MessageDirection direction, MessageCategory category)
        {
            mDirection = direction;
            mCategory = category;
        }

        public MessageDirection Direction
        {
            get { return mDirection; }
        }

        public MessageCategory Category
        {
            get { return mCategory; }
        }
    }
}