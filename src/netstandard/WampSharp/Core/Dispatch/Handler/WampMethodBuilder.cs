using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Logging;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;

namespace WampSharp.Core.Dispatch.Handler
{
    /// <summary>
    /// An implementation of <see cref="IMethodBuilder{TKey,TMethod}"/>.
    /// Builds efficient delegates using compiled expressions.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    public class WampMethodBuilder<TMessage, TClient> : IMethodBuilder<WampMethodInfo, Action<TClient, WampMessage<TMessage>>>
    {
        #region Members

        private readonly ILog mLogger;
        
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
            mLogger = LogProvider.GetLogger(this.GetType());
            mFormatter = formatter;
        }

        #endregion

        #region Public Methods

        public Action<TClient, WampMessage<TMessage>> BuildMethod(WampMethodInfo wampMethod)
        {
            Func<object, object[], object> action = BuildAction(wampMethod);

            return (client, message) =>
            {
                object[] arguments = GetArguments(client, message, wampMethod);
                object instance = GetInstance(client, message, wampMethod);
                action(instance, arguments);
            };
        }

        private Func<object, object[], object> BuildAction(WampMethodInfo wampMethod)
        {
            Func<object, object[], object> method = 
                MethodInvokeGenerator.CreateInvokeMethod(wampMethod.Method);

            return method;
        }

        #endregion

        #region Protected Members

        // Maybe these should be in a seperated interface.
        protected virtual object GetInstance(TClient client, WampMessage<TMessage> message, WampMethodInfo method)
        {
            return mInstance;
        }

        protected virtual object[] GetArguments(TClient client, WampMessage<TMessage> message, WampMethodInfo method)
        {
            return InnerGetArguments(client, message, method);
        }

        #endregion

        #region Private Members

        private object[] InnerGetArguments(TClient client, WampMessage<TMessage> message, WampMethodInfo method)
        {
            List<object> methodArguments = new List<object>(method.TotalArgumentsCount);

            if (method.HasWampClientArgument)
            {
                methodArguments.Add(client);
            }

            if (method.IsRawMethod)
            {
                methodArguments.Add(message);
            }
            else
            {
                methodArguments.AddRange
                    (ConvertArguments(message, method));                
            }

            return methodArguments.ToArray();
        }

        private object[] ConvertArguments(WampMessage<TMessage> message, WampMethodInfo method)
        {
            ParameterInfo[] parametersList = method.ParametersToConvert;

            TMessage[] arguments = message.Arguments;
            
            IEnumerable<TMessage> relevantArguments = arguments;

            if (method.HasParamsArgument)
            {
                relevantArguments = relevantArguments.Take(parametersList.Length - 1);
            }

            List<object> converted =
                parametersList.Zip(relevantArguments,
                                   (parameter, argument) =>
                                   DeserializeArgument(parameter, argument))
                              .ToList();

            if (method.HasParamsArgument)
            {
                IEnumerable<TMessage> otherArgumets =
                    arguments.Skip(parametersList.Length - 1);

                converted.Add(otherArgumets.ToArray());
            }

            return converted.ToArray();
        }

        private object DeserializeArgument(ParameterInfo parameter, TMessage argument)
        {
            try
            {
                return mFormatter.Deserialize(parameter.ParameterType, argument);
            }
            catch (Exception ex)
            {
                mLogger.ErrorFormat(ex, "Failed deserializing {ParameterName}", parameter.Name);
                throw;
            }
        }

        #endregion
    }
}