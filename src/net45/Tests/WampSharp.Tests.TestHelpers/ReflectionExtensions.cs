using System;
using System.Reflection;

namespace WampSharp.Tests.TestHelpers
{
#if NETCORE

    internal static class ReflectionExtensions
    {
        public static bool IsDefined(this Type type, Type attributeType, bool inherit)
        {
            return type.GetTypeInfo().IsDefined(attributeType, inherit);
        }

        public static bool IsInstanceOfType(this Type type, object value)
        {
            return type.GetTypeInfo().IsInstanceOfType(value);
        }
    }

#endif
}