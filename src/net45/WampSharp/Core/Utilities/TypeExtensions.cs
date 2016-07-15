using System.Collections.Generic;
using System.Linq;

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
#if !PCL
            return type.IsValueType;
#else
            return type.GetTypeInfo().IsValueType;
#endif
        }

        public static IEnumerable<EventInfo> GetInstanceEvents(this Type type)
        {
#if PCL
            return type.GetTypeInfo().DeclaredEvents.Where
                (x =>
                {
                    MethodInfo addMethod = x.AddMethod;
                    return addMethod.IsPublic && !addMethod.IsStatic;
                });
#else
            return type.GetEvents(BindingFlags.Public | BindingFlags.Instance);
#endif
        }

        public static IEnumerable<MethodInfo> GetPublicInstanceMethods(this Type type)
        {
#if PCL
            return type.GetTypeInfo().DeclaredMethods
                .Where(x => x.IsPublic && !x.IsStatic);
#else
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
#endif
        }

        public static IEnumerable<MethodInfo> GetInstanceMethods(this Type type)
        {
#if PCL
            return type.GetTypeInfo().DeclaredMethods
                .Where(x => !x.IsStatic);
#else
            return type.GetMethods(BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance);
#endif
        }


        public static bool IsGenericType(this Type type)
        {
#if !PCL
            return type.IsGenericType;
#else
            return type.GetTypeInfo().IsGenericType;
#endif
        }

        public static bool IsInterface(this Type type)
        {
#if !PCL
            return type.IsInterface;
#else
            return type.GetTypeInfo().IsInterface;
#endif
        }

        public static Type BaseType(this Type type)
        {
#if !PCL
            return type.BaseType;
#else
            return type.GetTypeInfo().BaseType;
#endif
        }

#if PCL && !NETCORE
        public static bool IsAssignableFrom(this Type interfaceType, Type type)
        {
            TypeInfo interfaceTypeInfo = interfaceType.GetTypeInfo();
            TypeInfo typeInfo = type.GetTypeInfo();

            return interfaceTypeInfo.IsAssignableFrom(typeInfo);
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GenericTypeArguments;
        }

        public static MethodInfo GetMethod(this Type type, string methodName)
        {
            return type.GetTypeInfo().GetDeclaredMethod(methodName);
        }

        public static IEnumerable<Type> GetInterfaces(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces;
        }

        public static bool IsInstanceOfType(this Type type, object instance)
        {
            return type.IsAssignableFrom(instance.GetType());
        }
#endif
    }
}