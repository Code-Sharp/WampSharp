using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WampSharp.Core.Utilities.ValueTuple;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.CodeGeneration
{
    internal class FormatTypeExtensions
    {
        public static string FormatType(Type type)
        {
            if (!type.IsGenericType && !type.IsArray)
            {
                return Prettify(type.FullName);
            }
            else if (type.IsArray)
            {
                return $"{FormatType(type.GetElementType())}[{new string(',', type.GetArrayRank() - 1)}]";
            }
            else if (type.IsGenericType)
            {
                string truncatedName = type.FullName.Substring(0, type.FullName.IndexOf('`'));

                return
                    $"{Prettify(truncatedName)}<{string.Join(", ", type.GetGenericArguments().Select(x => FormatType(x)))}>";
            }

            return null;
        }

        private static string Prettify(string fullName)
        {
            string result = fullName;

            switch (fullName)
            {
                case "System.Boolean":
                    result = "bool";
                    break;
                case "System.Char":
                    result = "char";
                    break;
                case "System.Byte":
                    result = "byte";
                    break;
                case "System.SByte":
                    result = "sbyte";
                    break;
                case "System.Int16":
                    result = "short";
                    break;
                case "System.UInt16":
                    result = "ushort";
                    break;
                case "System.Int32":
                    result = "int";
                    break;
                case "System.UInt32":
                    result = "uint";
                    break;
                case "System.Int64":
                    result = "long";
                    break;
                case "System.UInt64":
                    result = "ulong";
                    break;
                case "System.String":
                    result = "string";
                    break;
                case "System.Single":
                    result = "float";
                    break;
                case "System.Double":
                    result = "double";
                    break;
                case "System.Decimal":
                    result = "decimal";
                    break;
                case "System.Object":
                    result = "object";
                    break;
                case "System.Void":
                    result = "void";
                    break;
                case "System.Threading.Tasks.Task":
                    result = "Task";
                    break;
                case "System.IProgress":
                    result = "IProgress";
                    break;
                default :
                   result = "global::" + result.Replace("+", ".");
                    break;
            }

            return result;
        }

        public static string GetFormattedReturnType(MethodInfo method)
        {
            if (!method.ReturnsTuple())
            {
                return FormatType(method.ReturnType);
            }
            else
            {
                Type returnType = TaskExtensions.UnwrapReturnType(method.ReturnType);

                IEnumerable<Type> tupleElementTypes =
                    returnType.GetValueTupleElementTypes();

                TupleElementNamesAttribute tupleElementNamesAttribute = 
                    method.ReturnParameter.GetCustomAttribute<TupleElementNamesAttribute>();

                IEnumerable<string> tupleElementsIdentifiers;

                IEnumerable<string> tupleElementTypesIdentifiers = tupleElementTypes
                    .Select(x => FormatType(x));

                if (tupleElementNamesAttribute == null)
                {
                    tupleElementsIdentifiers = tupleElementTypesIdentifiers;
                }
                else
                {
                    tupleElementsIdentifiers =
                        tupleElementTypesIdentifiers
                            .Zip(tupleElementNamesAttribute.TransformNames,
                                 (type, name) => $"{type} {name}");
                }

                string tupleIdentifierContent = string.Join(", ", tupleElementsIdentifiers);

                if (typeof(Task).IsAssignableFrom(method.ReturnType))
                {
                    return $"Task<({tupleIdentifierContent})>";
                }
                else
                {
                    return $"({tupleIdentifierContent})";
                }
            }
        }
    }
}