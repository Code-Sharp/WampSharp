using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

namespace WampSharp.Core.Dispatch.Handler
{
    public class WampMethodBuilder<TMessage, TClient> : IMethodBuilder<WampMethodInfo, Action<TClient, WampMessage<TMessage>>>
    {
        private readonly object mInstance;
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampMethodBuilder(object instance, IWampFormatter<TMessage> formatter)
        {
            mInstance = instance;
            mFormatter = formatter;
        }

        public Action<TClient, WampMessage<TMessage>> BuildMethod(WampMethodInfo wampMethod)
        {
            MethodInfo method = wampMethod.Method;

            return (client, message) =>
                   method.Invoke(mInstance, GetArguments(client, message, wampMethod));
        }

        private object[] GetArguments(TClient client, WampMessage<TMessage> message, WampMethodInfo method)
        {
            ParameterInfo[] parameterInfos = method.Method.GetParameters();

            IEnumerable<ParameterInfo> parametersToConvert = parameterInfos;

            IEnumerable<object> methodArguments = Enumerable.Empty<object>();

            if (method.HasWampClientArgument)
            {
                methodArguments = new object[] { client };
                parametersToConvert = parametersToConvert.Skip(1);
            }

            if (method.IsRawMethod)
            {
                methodArguments = methodArguments.Concat(new[] {message});
            }
            else
            {
                methodArguments =
                    methodArguments.Concat(ConvertArguments(message, parametersToConvert));                
            }

            return methodArguments.ToArray();
        }

        private object[] ConvertArguments(WampMessage<TMessage> message, IEnumerable<ParameterInfo> parameters)
        {
            List<ParameterInfo> parametersList = parameters.ToList();

            TMessage[] arguments = message.Arguments;
            
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