using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

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
            Action<object, object[]> action = BuildAction(wampMethod);

            return (client, message) =>
                   action(GetInstance(client, message, wampMethod), GetArguments(client, message, wampMethod));
        }

        private Action<object, object[]> BuildAction(WampMethodInfo wampMethod)
        {
            MethodInfo method = wampMethod.Method;

            ParameterInfo[] parameters = wampMethod.Parameters;

            ParameterExpression instance =
                Expression.Parameter(typeof (object), "instance");

            ParameterExpression arguments =
                Expression.Parameter(typeof (object[]), "arguments");

            IEnumerable<Expression> converted =
                parameters.Select((x, i) =>
                                  Expression.Convert
                                      (Expression.ArrayIndex(arguments,
                                                             Expression.Constant(i)),
                                       x.ParameterType))
                          .ToArray();

            MethodCallExpression body =
                Expression.Call(Expression.Convert(instance, method.DeclaringType),
                                method,
                                converted);

            Expression<Action<object, object[]>> lambda =
                Expression.Lambda<Action<object, object[]>>(body, instance, arguments);

            return lambda.Compile();
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
                                   mFormatter.Deserialize(parameter.ParameterType, argument))
                              .ToList();

            if (method.HasParamsArgument)
            {
                IEnumerable<TMessage> otherArgumets =
                    arguments.Skip(parametersList.Length - 1);

                converted.Add(otherArgumets.ToArray());
            }

            return converted.ToArray();
        }

        #endregion
    }
}