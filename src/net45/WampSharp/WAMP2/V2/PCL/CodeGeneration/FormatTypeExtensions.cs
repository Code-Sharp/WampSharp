#if PCL
using System;
using System.Linq;
using System.Reflection;

namespace WampSharp.CodeGeneration
{
    internal class FormatTypeExtensions
    {
        public static string FormatType(Type type)
        {
            if (!type.IsGenericType() && !type.IsArray)
            {
                return Prettify(type.FullName);
            }
            else if (type.IsArray)
            {
                return string.Format("{0}[{1}]", FormatType(type.GetElementType()),
                    new string(',', type.GetArrayRank() - 1));
            }
            else if (type.IsGenericType())
            {
                string truncatedName = type.FullName.Substring(0, type.FullName.IndexOf('`'));

                return string.Format("{0}<{1}>", Prettify(truncatedName),
                    string.Join(", ", type.GetGenericArguments().Select(x => FormatType(x))));
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
    }
}
#endif