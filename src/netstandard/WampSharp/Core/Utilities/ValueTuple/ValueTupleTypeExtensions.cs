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
            if (!type.IsGenericType)
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
                throw new ArgumentException("Expected a ValueTuple type", nameof(type));
            }

            Type genericTypeDefinition = type.GetGenericTypeDefinition();

            Type[] genericArguments = genericTypeDefinition.GetGenericArguments();

            int tupleLength = genericArguments.Length;
            if (!genericTypeDefinition.IsLongTuple())
            {
                return tupleLength;
            }
            else
            {
                Type last = type.GetGenericArguments().Last();

                return (tupleLength - 1) +
                       last.GetValueTupleLength();
            }
        }

        public static bool IsLongTuple(this Type tupleType)
        {
            return tupleType.IsGenericType &&
                   tupleType.GetGenericTypeDefinition() == typeof(ValueTuple<,,,,,,,>);
        }

        public static bool IsValidTupleType(this Type tupleType)
        {
            if (tupleType.IsValueTuple())
            {
                if (tupleType.IsLongTuple())
                {
                    Type rest = tupleType.GetGenericArguments().Last();

                    if (!rest.IsValidTupleType())
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
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