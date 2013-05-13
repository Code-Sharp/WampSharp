using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

namespace WampSharp.Core.Dispatch.Handler
{
    /// <summary>
    /// An implementation of <see cref="IMethodBuilder{TKey,TMethod}"/>.
    /// Builds non efficient delegates using <see cref="MethodInfo"/>'s invoke.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    public class WampMethodBuilder<TMessage, TClient> : IMethodBuilder<WampMethodInfo, Action<TClient, WampMessage<TMessage>>>
    {
        #region Members

        private readonly object mInstance;
        private readonly IWampFormatter<TMessage> mFormatter;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WampMethodBuilder{TMessage,TClient}"/>.
        /// </summary>
        /// <param name="instance">The instance to call its methods.</param>
        /// <param name="formatter">The <see cref="IWampFormatter{TMessage}"/> used to convert the arguments.</param>
        public WampMethodBuilder(object instance, IWampFormatter<TMessage> formatter)
        {
            mInstance = instance;
            mFormatter = formatter;
        }

        #endregion

        #region Public Methods

        public Action<TClient, WampMessage<TMessage>> BuildMethod(WampMethodInfo wampMethod)
        {
            MethodInfo method = wampMethod.Method;

            return (client, message) =>
                   method.Invoke(mInstance, GetArguments(client, message, wampMethod));
        }

        #endregion

        #region Private Members

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

        #endregion
    }
}