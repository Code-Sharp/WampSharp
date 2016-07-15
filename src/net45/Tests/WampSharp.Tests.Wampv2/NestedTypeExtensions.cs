#if NETCORE
using System;
using System.Reflection;

namespace WampSharp.Tests.Wampv2
{
    internal static class NestedTypeExtensions
    {
        public static Type[] GetNestedTypes(this Type type)
        {
            return type.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
        }

        public static Type GetNestedType(this Type type, string name)
        {
            return type.GetNestedType(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
        }
    }
}

#endif