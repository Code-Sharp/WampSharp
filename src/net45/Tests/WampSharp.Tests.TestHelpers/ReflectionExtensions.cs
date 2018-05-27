using System;
using System.Reflection;

namespace WampSharp.Tests.TestHelpers
{
#if NETCORE && !NETSTANDARD2_0

    internal static class ReflectionExtensions
    {
        public static bool IsDefined(this Type type, Type attributeType, bool inherit)
        {
            return type.GetTypeInfo().IsDefined(attributeType, inherit);
        }
    }

#endif
}