using System;

namespace WampSharp.V2.Rpc
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