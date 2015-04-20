using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.CodeGeneration
{
    public class CalleeProxyCodeGenerator
    {
        private readonly string mNamespaceName;

        private readonly string mClassDeclarationTemplate =
@"using System;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.CalleeProxy;
using WampSharp.V2.Rpc;

namespace {$namespace}
{
    public class {$proxyName}Proxy : CalleeProxyBase, {$implementedInterface}
    {
        public {$proxyName}Proxy(IWampChannel channel, ICalleeProxyInterceptor interceptor)
            : base(channel, interceptor)
        {
        }
{$implementedMethods}
    }
}";
        public CalleeProxyCodeGenerator(string namespaceName)
        {
            mNamespaceName = namespaceName;
        }

        public string GenerateCode(Type interfaceType)
        {
            ProxyMethodWriter writer = new ProxyMethodWriter();

            List<string> methods = new List<string>();

            foreach (MethodInfo method in interfaceType.GetMethods())
            {
                string methodCode = writer.WriteMethod(method);
                methods.Add(methodCode);
            }

            string joinedMethods = 
                string.Join(Environment.NewLine, methods);

            joinedMethods =
                string.Join(Environment.NewLine,
                            joinedMethods.Split(new[] {Environment.NewLine}, StringSplitOptions.None)
                                         .Select(x => "        " + x));

            IDictionary<string, string> dictionary = 
                new Dictionary<string, string>();

            dictionary["namespace"] = mNamespaceName;
            dictionary["implementedMethods"] = joinedMethods;
            dictionary["implementedInterface"] = FormatTypeExtensions.FormatType(interfaceType);
            dictionary["proxyName"] = GetInterfaceName(interfaceType);

            string processed = TemplateHelper.ProcessTemplate(mClassDeclarationTemplate, dictionary);

            return processed;
        }

        private string GetInterfaceName(Type interfaceType)
        {
            string result = interfaceType.Name;

            if (result.StartsWith("I"))
            {
                return result.Substring(1);
            }
            else
            {
                return result;
            }
        }
    }

    public interface IProxyMethodWriter
    {
        string WriteMethod(MethodInfo method);
    }

    class ProxyMethodWriter : IProxyMethodWriter
    {
        private string mTemplate =
@"
[WampProcedure(""{$procedureUri}"")]
public {$returnType} {$methodName}({$parametersDeclaration})
{
    {$return}{$invokeMethod}{$genericType}({$parameterList});
}";

        public string WriteMethod(MethodInfo method)
        {
            WampProcedureAttribute attribute = 
                method.GetCustomAttribute<WampProcedureAttribute>(true);

            IDictionary<string, string> dictionary =
                new Dictionary<string, string>();

            ParameterInfo[] parameters = method.GetParameters();
            
            dictionary["procedureUri"] = attribute.Procedure;
            dictionary["methodName"] = method.Name;
            dictionary["parameterList"] =
                string.Join(", ",
                            new[] {"MethodBase.GetCurrentMethod()"}.Concat(parameters.Select(x => x.Name)));

            dictionary["returnType"] = FormatTypeExtensions.FormatType(method.ReturnType);

            string invokeMethod;

            if (method.GetCustomAttribute<WampProgressiveResultProcedureAttribute>() != null)
            {
                invokeMethod = "InvokeProgressiveAsync";

                dictionary["parameterList"] =
                    string.Join(", ",
                                new[] {"MethodBase.GetCurrentMethod()"}.Concat
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
                dictionary["genericType"] = GetGenericType(returnType);
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
                dictionary["genericType"] = GetGenericType(returnType.GetElementType());
            }

            dictionary["invokeMethod"] = invokeMethod;

            dictionary["parametersDeclaration"] =
                string.Join(", ",
                            parameters.Select
                                (x => FormatTypeExtensions.FormatType(x.ParameterType) + " " + x.Name));

            return TemplateHelper.ProcessTemplate(mTemplate, dictionary);
        }

        private static string GetGenericType(Type returnType)
        {
            return string.Format("<{0}>", FormatTypeExtensions.FormatType(returnType));
        }
    }
}