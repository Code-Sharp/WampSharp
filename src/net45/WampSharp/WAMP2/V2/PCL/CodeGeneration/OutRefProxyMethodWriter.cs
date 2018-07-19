using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Utilities;

namespace WampSharp.CodeGeneration
{
    internal class OutRefProxyMethodWriter : IProxyMethodWriter
    {
        private readonly string mFieldDeclaration =
@"
private static readonly InvokeSyncDelegate<{$genericType}> mMethodHandler{$methodIndex} =
    GetInvokeSync<{$genericType}>(
        GetMethodInfo(() =>
        {    
{$variableDefaultAssignments}

            return (Expression<Action<{$interfaceType}>>)
                (x => x.{$methodName}({$qualifiedVariableList}));
        })
);";

        private readonly string mDefaultAssignment =
"            {$parameterType} {$parameterName} = default({$parameterType});";

        public string WriteField(int methodIndex, MethodInfo method)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            Type type = method.DeclaringType;

            dictionary["methodIndex"] = methodIndex.ToString();
            dictionary["interfaceType"] = FormatTypeExtensions.FormatType(type);
            Type returnType = TaskExtensions.UnwrapReturnType(method.ReturnType);
            dictionary["genericType"] = FormatTypeExtensions.FormatType(returnType);
            dictionary["methodName"] = method.Name;
            dictionary["qualifiedVariableList"] =
                string.Join(", ",
                            method.GetParameters()
                                  .Select(x => GetQualifiedName(x,
                                                                parameter => parameter.Name)));

            dictionary["variableDefaultAssignments"] =
                string.Join(Environment.NewLine,
                            method.GetParameters().
                                   Select(parameter => GetDefaultAssignment(parameter)));

            return CodeGenerationHelper.ProcessTemplate(mFieldDeclaration, dictionary);
        }

        private string GetDefaultAssignment(ParameterInfo parameter)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary["parameterType"] = FormatTypeExtensions.FormatType(parameter.ParameterType.StripByRef());
            dictionary["parameterName"] = parameter.Name;

            return CodeGenerationHelper.ProcessTemplate(mDefaultAssignment, dictionary);
        }

        private string GetQualifiedName(ParameterInfo parameterInfo, 
            Func<ParameterInfo, string> parameterNameProvider)
        {
            string name = parameterNameProvider(parameterInfo);

            if (parameterInfo.IsOut)
            {
                return "out " + name;
            }

            if (parameterInfo.ParameterType.IsByRef)
            {
                return "ref " + name;
            }

            return name;
        }


        private string mMethodTemplate =
@"
public {$returnType} {$methodName}({$parametersDeclaration})
{
    object[] ___array = new object[] { {$array} };
    {$varResult}{$methodHandler}({$parameterList});
{$unpack}
    {$return}
}";

        public string WriteMethod(int methodIndex, MethodInfo method)
        {
            string methodHandler = "mMethodHandler" + methodIndex;
            
            IDictionary<string, string> dictionary =
                new Dictionary<string, string>();

            ParameterInfo[] parameters = method.GetParameters();

            dictionary["array"] =
                string.Join(", ",
                            (parameters.Select(x => GetCallerParameter(x))));


            dictionary["methodName"] = method.Name;
         
            dictionary["parameterList"] =
                string.Join(", ",
                            new[] {"this", "___array"});

            Type returnType = method.ReturnType;
            dictionary["returnType"] = FormatTypeExtensions.GetFormattedReturnType(method);

            if (returnType != typeof(void))
            {
                dictionary["varResult"] = "var ___result = ";
                dictionary["return"] = "return ___result;";
            }
            else
            {
                dictionary["varResult"] = string.Empty;
                dictionary["return"] = "return;";
            }

            dictionary["parametersDeclaration"] =
                string.Join(", ",
                            parameters.Select
                                (x => 
                                    GetQualifiedName(x, CodeGenerationHelper.GetMethodParameterDeclaration)));

            dictionary["unpack"] = 
                string.Join(Environment.NewLine,
                parameters.Where(x => x.IsOut || x.ParameterType.IsByRef)
                .Select(x => GetUnpackStatement(x)));

            dictionary["methodHandler"] = methodHandler;

            return CodeGenerationHelper.ProcessTemplate(mMethodTemplate, dictionary);
        }

        private readonly string mUnpackTemplate =
            "    {$parameterName} = ({$parameterType}) ___array[{$parameterIndex}];";

        private string GetUnpackStatement(ParameterInfo parameterInfo)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary["parameterType"] = FormatTypeExtensions.FormatType(parameterInfo.ParameterType.StripByRef());
            dictionary["parameterName"] = parameterInfo.Name;
            dictionary["parameterIndex"] = parameterInfo.Position.ToString();

            return CodeGenerationHelper.ProcessTemplate(mUnpackTemplate, dictionary);
        }

        private string GetCallerParameter(ParameterInfo parameter)
        {
            if (parameter.IsOut)
            {
                return "null";
            }
            else
            {
                return parameter.Name;
            }
        }
    }
}