using System;
using System.Collections.Generic;
using System.Reflection;

namespace WampSharp.CodeGeneration
{
    internal static class CodeGenerationHelper
    {
        public static string GetGenericType(Type returnType)
        {
            return $"<{FormatTypeExtensions.FormatType(returnType)}>";
        }

        public static string GetMethodParameterDeclaration(ParameterInfo parameter)
        {
            return FormatTypeExtensions.FormatType(parameter.ParameterType.StripByRef()) + " " + parameter.Name;
        }

        public static string ProcessTemplate(string template, IDictionary<string, string> dictionary)
        {
            string result = template;

            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                result = result.Replace($"{{${keyValuePair.Key}}}",
                                        keyValuePair.Value);
            }

            return result;
        }
    }
}