using System;

namespace WampSharp.Core.Utilities
{
    internal static class TypeExtensions
    {
        public static Type StripByRef(this Type type)
        {
            if (type.IsByRef)
            {
                return type.GetElementType();
            }

            return type;
        }
    }
}