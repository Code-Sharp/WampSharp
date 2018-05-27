namespace WampSharp.Tests.TestHelpers
{
#if NETCORE

    internal static class ReflectionExtensions
    {
        public static bool IsDefined(this Type type, Type attributeType, bool inherit)
        {
            return type.GetTypeInfo().IsDefined(attributeType, inherit);
        }
    }

#endif
}