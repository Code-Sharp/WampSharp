using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Contracts;
using WampSharp.Core.Serialization;

namespace WampSharp.Core.Dispatch.Handler
{
    public class WampMethodBuilder<TMessage> : IMethodBuilder<WampMethodInfo, Action<IWampClient, TMessage[]>>
    {
        private readonly object mInstance;
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampMethodBuilder(object instance, IWampFormatter<TMessage> formatter)
        {
            mInstance = instance;
            mFormatter = formatter;
        }

        public Action<IWampClient, TMessage[]> BuildMethod(WampMethodInfo wampMethod)
        {
            MethodInfo method = wampMethod.Method;

            return (client, arguments) =>
                   method.Invoke(mInstance, GetArguments(client, arguments, wampMethod));
        }

        private object[] GetArguments(IWampClient client, TMessage[] arguments, WampMethodInfo method)
        {
            ParameterInfo[] parameterInfos = method.Method.GetParameters();

            IEnumerable<ParameterInfo> parametersToConvert = parameterInfos;

            IEnumerable<object> methodArguments = Enumerable.Empty<object>();

            if (method.HasWampClientArgument)
            {
                methodArguments = new object[] { client };
                parametersToConvert = parametersToConvert.Skip(1);
            }

            methodArguments =
                methodArguments.Concat(ConvertArguments(arguments, parametersToConvert));

            return methodArguments.ToArray();
        }

        private object[] ConvertArguments(TMessage[] arguments, IEnumerable<ParameterInfo> parameters)
        {
            var parametersList = parameters.ToList();

            IEnumerable<TMessage> relevantArguments = arguments;

            bool paramsArgument =
                parametersList.Last().IsDefined(typeof(ParamArrayAttribute));

            if (paramsArgument)
            {
                relevantArguments = relevantArguments.Take(parametersList.Count - 1);
            }

            List<object> converted =
                parametersList.Zip(relevantArguments,
                                   (parameter, argument) =>
                                   mFormatter.Deserialize(parameter.ParameterType, argument))
                              .ToList();

            if (paramsArgument)
            {
                IEnumerable<TMessage> otherArgumets =
                    arguments.Skip(parametersList.Count - 1);

                converted.Add(otherArgumets.ToArray());
            }

            return converted.ToArray();
        }
    }
}