using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WampSharp.Core.Utilities.ValueTuple
{
    internal static class ValueTupleTypeExtensions
    {
        public static bool IsValueTuple(this Type type)
        {
            if (!type.IsGenericType())
            {
                return false;
            }

            Type genericTypeDefinition = type.GetGenericTypeDefinition();

            if ((genericTypeDefinition == typeof(ValueTuple<>)) ||
                (genericTypeDefinition == typeof(ValueTuple<,>)) ||
                (genericTypeDefinition == typeof(ValueTuple<,,>)) ||
                (genericTypeDefinition == typeof(ValueTuple<,,,>)) ||
                (genericTypeDefinition == typeof(ValueTuple<,,,,>)) ||
                (genericTypeDefinition == typeof(ValueTuple<,,,,,>)) ||
                (genericTypeDefinition == typeof(ValueTuple<,,,,,,>)) ||
                (genericTypeDefinition == typeof(ValueTuple<,,,,,,,>)))
            {
                return true;
            }

            return false;
        }

        public static int GetValueTupleLength(this Type type)
        {
            if (!type.IsValueTuple())
            {
                throw new ArgumentException("Expected a ValueTuple type", "type");
            }

            Type genericTypeDefinition = type.GetGenericTypeDefinition();

            if (genericTypeDefinition == typeof(ValueTuple<>))
            {
                return 1;
            }
            if (genericTypeDefinition == typeof(ValueTuple<,>))
            {
                return 2;
            }
            if (genericTypeDefinition == typeof(ValueTuple<,,>))
            {
                return 3;
            }
            if (genericTypeDefinition == typeof(ValueTuple<,,,>))
            {
                return 4;
            }
            if (genericTypeDefinition == typeof(ValueTuple<,,,,>))
            {
                return 5;
            }
            if (genericTypeDefinition == typeof(ValueTuple<,,,,,>))
            {
                return 6;
            }
            if (genericTypeDefinition == typeof(ValueTuple<,,,,,,>))
            {
                return 7;
            }
            if (genericTypeDefinition == typeof(ValueTuple<,,,,,,,>))
            {
                Type last = type.GetGenericArguments().Last();

                if (!last.IsValueTuple())
                {
                    return 8;
                }
                else
                {
                    return 7 + last.GetValueTupleLength();
                }
            }

            return 0;
        }

        public static bool IsLongTuple(this Type tupleType)
        {
            return tupleType.IsGenericType() &&
                   tupleType.GetGenericTypeDefinition() == typeof(ValueTuple<,,,,,,,>) &&
                   tupleType.GetGenericArguments().Last().IsValueTuple();
        }

        public static IEnumerable<Type> GetValueTupleElementTypes(this Type tupleType)
        {
            Type[] genericArguments = tupleType.GetGenericArguments();

            if (!tupleType.IsLongTuple())
            {
                return genericArguments;
            }
            else
            {
                IEnumerable<Type> firstElements = 
                    genericArguments.Take(genericArguments.Length - 1);

                Type nestedTupleType = genericArguments.Last();

                IEnumerable<Type> rest = nestedTupleType.GetValueTupleElementTypes();

                return firstElements.Concat(rest);
            }
        }

        public static bool ReturnsTuple(this MethodInfo method)
        {
            Type unwrappedReturnType = TaskExtensions.UnwrapReturnType(method.ReturnType);

            return unwrappedReturnType.IsValueTuple();
        }
    }
}