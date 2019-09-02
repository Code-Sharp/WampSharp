using System.Collections.Generic;

namespace System.Reflection
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

        public static IEnumerable<EventInfo> GetPublicInstanceEvents(this Type type)
        {
            return type.GetEvents(BindingFlags.Public | BindingFlags.Instance);
        }

        public static IEnumerable<MethodInfo> GetPublicInstanceMethods(this Type type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        }

        public static IEnumerable<MethodInfo> GetInstanceMethods(this Type type)
        {
            return type.GetMethods(BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);
        }
    }
}