using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace WampSharp.Core.Utilities
{
    /// <summary>
    /// Contains extension methods of <see cref="Type"/> for 
    /// generic types.
    /// </summary>
    public static class GenericTypeExtensions
    {
        /// <summary>
        /// Returns a value indicating whether the given type
        /// can be converted to a generic version of the given open generic type.
        /// </summary>
        /// <param name="openGenericType">The given open generic type.</param>
        /// <param name="type">The given type.</param>
        /// <returns>A value indicating whether the given type
        /// can be converted to a generic version of the given open generic type.</returns>
        public static bool IsGenericAssignableFrom(this Type openGenericType,
                                                   Type type)
        {
            return GetClosedGenericTypeImplementation(type, openGenericType) != null;
        }

        /// <summary>
        /// Returns the closed generic type of the given generic open type,
        /// that the given type is assignable to.
        /// </summary>
        /// <param name="type">The given type.</param>
        /// <param name="openGenericType">The open generic type.</param>
        /// <returns>The closed generic type of the given generic open type,
        /// that the given type is assignable to.</returns>
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
                           .Select(x => GetClosedGenericTypeImplementation(x, openGenericType))
                           .FirstOrDefault(x => x != null);
            }
        }

        public static Type GetProgressParameterType(this MethodInfo method)
        {
            ParameterInfo progressParameter = GetProgressParameter(method);

            return progressParameter.ParameterType.GetGenericArguments()[0];
        }

        public static ParameterInfo GetProgressParameter(this MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            ParameterInfo lastParameter = parameters.LastOrDefault();
            ParameterInfo progressParameter = lastParameter;

            if ((lastParameter != null) &&
                (lastParameter.ParameterType == typeof(CancellationToken)))
            {
                progressParameter =
                    parameters.Take(parameters.Length - 1).LastOrDefault();
            }

            return progressParameter;
        }
    }
}
