#if !CASTLE && !DISPATCH_PROXY
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
    {$return}{$methodHandler}({$parameterList});
}";

        private string GetDelegateType(MethodInfo method)
        {
            string prefix = GetDelegateNamePrefix(method);

            Type returnType = TaskExtensions.UnwrapReturnType(method.ReturnType);

            string returnTypeAlias = FormatTypeExtensions.FormatType(returnType);

            string result = string.Format("{0}Delegate<{1}>", prefix, returnTypeAlias);

            return result;
        }

        private static string GetDelegateNamePrefix(MethodInfo method)
        {
            string result;

            if (method.GetCustomAttribute<WampProgressiveResultProcedureAttribute>() != null)
            {
                result = "InvokeProgressiveAsync";
            }
            else if (typeof(Task).IsAssignableFrom(method.ReturnType))
            {
                result = "InvokeAsync";
            }
            else
            {
                result = "InvokeSync";
            }

            return result;
        }

        public string WriteMethod(int methodIndex, MethodInfo method)
        {
            string methodHandler = "mMethodHandler" + methodIndex;
            
            IDictionary<string, string> dictionary =
                new Dictionary<string, string>();

            ParameterInfo[] parameters = method.GetParameters();

            dictionary["methodName"] = method.Name;
            dictionary["parameterList"] =
                string.Join(", ", new [] {"this"}.Concat(parameters.Select(x => x.Name)));

            dictionary["returnType"] = FormatTypeExtensions.FormatType(method.ReturnType);

            if (method.GetCustomAttribute<WampProgressiveResultProcedureAttribute>() != null)
            {
                dictionary["parameterList"] =
                    string.Join(", ",
                                new [] {"this"}.Concat(new[] { parameters.Last() }.Concat(parameters.Take(parameters.Length - 1))
                                                         .Select(x => x.Name)));
            }

            if (method.ReturnType != typeof(void))
            {
                dictionary["return"] = "return ";
            }
            else
            {
                dictionary["return"] = string.Empty;
            }

            dictionary["methodHandler"] = methodHandler;

            dictionary["parametersDeclaration"] =
                string.Join(", ",
                            parameters.Select
                                (x => FormatTypeExtensions.FormatType(x.ParameterType) + " " + x.Name));

            return CodeGenerationHelper.ProcessTemplate(mMethodTemplate, dictionary);
        }

        private string mFieldTemplate =
            @"private static readonly {$delegateType} mMethodHandler{$methodIndex} = Get{$delegatePrefix}<{$genericType}>(
    GetMethodInfo(({$interfaceType} instance) => instance.{$methodName}({$defaults}))
);";

        public string WriteField(int methodIndex, MethodInfo method)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            Type type = method.DeclaringType;

            dictionary["methodIndex"] = methodIndex.ToString();
            dictionary["delegateType"] = GetDelegateType(method);
            dictionary["delegatePrefix"] = GetDelegateNamePrefix(method);
            dictionary["interfaceType"] = FormatTypeExtensions.FormatType(type);
            Type genericType = TaskExtensions.UnwrapReturnType(method.ReturnType);
            dictionary["genericType"] = FormatTypeExtensions.FormatType(genericType);
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