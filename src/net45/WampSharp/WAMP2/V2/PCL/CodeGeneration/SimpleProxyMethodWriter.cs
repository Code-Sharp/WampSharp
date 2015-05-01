#if PCL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.CodeGeneration
{
    internal class SimpleProxyMethodWriter : IProxyMethodWriter
    {
        private string mMethodTemplate =
            @"
public {$returnType} {$methodName}({$parametersDeclaration})
{
    {$return}{$invokeMethod}{$genericType}({$parameterList});
}";

        public string WriteMethod(int methodIndex, MethodInfo method)
        {
            string methodField = "mMethod" + methodIndex;
            
            IDictionary<string, string> dictionary =
                new Dictionary<string, string>();

            ParameterInfo[] parameters = method.GetParameters();

            dictionary["methodName"] = method.Name;
            dictionary["parameterList"] =
                string.Join(", ",
                            new[] {methodField}.Concat(parameters.Select(x => x.Name)));

            dictionary["returnType"] = FormatTypeExtensions.FormatType(method.ReturnType);

            string invokeMethod;

            if (method.GetCustomAttribute<WampProgressiveResultProcedureAttribute>() != null)
            {
                invokeMethod = "InvokeProgressiveAsync";

                dictionary["parameterList"] =
                    string.Join(", ",
                                new[] {methodField}.Concat
                                    (new[] {parameters.Last()}.Concat(parameters.Take(parameters.Length - 1))
                                                              .Select(x => x.Name)));
            }
            else if (typeof (Task).IsAssignableFrom(method.ReturnType))
            {
                invokeMethod = "InvokeAsync";
            }
            else
            {
                invokeMethod = "InvokeSync";
            }

            Type returnType = TaskExtensions.UnwrapReturnType(method.ReturnType);

            if (method.ReturnType != typeof(void))
            {
                dictionary["return"] = "return ";
                dictionary["genericType"] = CodeGenerationHelper.GetGenericType(returnType);
            }
            else
            {
                dictionary["return"] = string.Empty;
                dictionary["genericType"] = string.Empty;
            }

            if (method.ReturnType == typeof (Task))
            {
                dictionary["genericType"] = string.Empty;                
            }

            if (!method.HasMultivaluedResult())
            {
                invokeMethod = "Single" + invokeMethod;
            }
            else
            {
                invokeMethod = "Multi" + invokeMethod;
                dictionary["genericType"] = CodeGenerationHelper.GetGenericType(returnType.GetElementType());
            }

            dictionary["invokeMethod"] = invokeMethod;

            dictionary["parametersDeclaration"] =
                string.Join(", ",
                            parameters.Select
                                (x => FormatTypeExtensions.FormatType(x.ParameterType) + " " + x.Name));

            return CodeGenerationHelper.ProcessTemplate(mMethodTemplate, dictionary);
        }

        private string mFieldTemplate =
            @"private static readonly MethodInfo mMethod{$methodIndex} = GetMethodInfo(({$interfaceType} instance) => instance.{$methodName}({$defaults}));";

        public string WriteField(int methodIndex, MethodInfo method)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            Type type = method.DeclaringType;

            dictionary["methodIndex"] = methodIndex.ToString();
            dictionary["interfaceType"] = FormatTypeExtensions.FormatType(type);
            dictionary["methodName"] = method.Name;
            dictionary["defaults"] =
                string.Join(", ",
                            method.GetParameters()
                                  .Select(x => string.Format("default({0})",
                                                             FormatTypeExtensions.FormatType(x.ParameterType))));

            return CodeGenerationHelper.ProcessTemplate(mFieldTemplate, dictionary);
        }
    }
}
#endif