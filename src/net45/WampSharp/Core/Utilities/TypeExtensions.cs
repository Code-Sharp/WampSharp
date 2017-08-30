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

        public static bool IsValueType(this Type type)
        {
            return type.IsValueType;
        }

        public static IEnumerable<EventInfo> GetInstanceEvents(this Type type)
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


        public static bool IsGenericType(this Type type)
        {
            return type.IsGenericType;
        }

        public static bool IsInterface(this Type type)
        {
            return type.IsInterface;
        }

        public static Type BaseType(this Type type)
        {
            return type.BaseType;
        }

        public static GenericParameterAttributes GenericParameterAttributes(this Type type)
        {
            return type.GenericParameterAttributes;
        }
    }
}