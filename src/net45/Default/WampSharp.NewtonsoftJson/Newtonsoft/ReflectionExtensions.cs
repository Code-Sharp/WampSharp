#if PCL
using System;
using System.Reflection;

namespace WampSharp.Newtonsoft
{
    internal static class ReflectionExtensions
    {
        public static bool IsAssignableFrom(this Type interfaceType, Type type)
        {
            TypeInfo interfaceTypeInfo = interfaceType.GetTypeInfo();
            TypeInfo typeInfo = type.GetTypeInfo();

            return interfaceTypeInfo.IsAssignableFrom(typeInfo);
        }
    }
}
#endif