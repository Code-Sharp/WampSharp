using System;
using System.Linq;

namespace WampSharp.Core.Dispatch.Handler
{
    public static class GenericTypeExtensions
    {
        public static bool IsAssignableFromGeneric(this Type openGenericType,
                                                   Type type)
        {
            return GetClosedGenericTypeImplementation(type, openGenericType) != null;
        }

        public static Type GetClosedGenericTypeImplementation(this Type type, Type openGenericType)
        {
            if (type == null)
            {
                return null;
            }

            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == openGenericType)
            {
                return type;
            }

            if (!openGenericType.IsInterface)
            {
                return type.BaseType.GetClosedGenericTypeImplementation(openGenericType);
            }
            else
            {
                return type.GetInterfaces()
                           .Select(x => x.GetClosedGenericTypeImplementation(openGenericType))
                           .FirstOrDefault(x => x != null);
            }
        }
    }
}
