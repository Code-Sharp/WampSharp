using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;
using WampSharp.Core.Utilities;

namespace WampSharp.Core.Dispatch.Handler
{
    /// <summary>
    /// Represents a structure that contains information about a
    /// <see cref="WampHandlerAttribute"/> method.
    /// </summary>
    public class WampMethodInfo
    {

        #region Members
        private readonly int mTotalArgumentsCount;
        private readonly bool mIsRawMethod;
        private readonly ParameterInfo[] mParametersToConvert;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new <see cref="WampMethodInfo"/> given a
        /// <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method">The given method to obtain information
        /// from.</param>
        public WampMethodInfo(MethodInfo method)
        {
            Method = method;

            WampHandlerAttribute handlerAttribute = 
                method.GetCustomAttribute<WampHandlerAttribute>(true);

            if (handlerAttribute != null)
            {
                MessageType = handlerAttribute.MessageType;
            }
            else
            {
                MessageType = WampMessageType.Unknown;
            }

            ParameterInfo[] parameters = method.GetParameters();

            Parameters = parameters;
            List<ParameterInfo> parametersToConvert = Parameters.ToList();

            if (parameters.Length > 0)
            {
                if (parameters[0].IsDefined(typeof(WampProxyParameterAttribute), true))
                {
                    HasWampClientArgument = true;
                    parametersToConvert.RemoveAt(0);
                }

                ParameterInfo lastParameter = parameters.Last();

                if (lastParameter.IsDefined(typeof(ParamArrayAttribute), true))
                {
                    HasParamsArgument = true;
                }

                if (typeof (WampMessage<>).IsGenericAssignableFrom(lastParameter.ParameterType))
                {
                    mIsRawMethod = true;
                    parametersToConvert.RemoveAt(parametersToConvert.Count - 1);
                }
            }

            mTotalArgumentsCount = parameters.Length;
            ArgumentsCount = mTotalArgumentsCount;
            mParametersToConvert = parametersToConvert.ToArray();

            if (HasWampClientArgument)
            {
                ArgumentsCount = ArgumentsCount - 1;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> this method represents.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Gets the number of arguments this method gets, not including
        /// <see cref="WampProxyParameterAttribute"/> parameter.
        /// </summary>
        public int ArgumentsCount { get; }

        /// <summary>
        /// Returns a value indicating whether this method gets a
        /// <see cref="WampProxyParameterAttribute"/> parameter.
        /// </summary>
        public bool HasWampClientArgument { get; }

        /// <summary>
        /// Returns a value indicating whether this method has a params
        /// argument.
        /// </summary>
        public bool HasParamsArgument { get; }

        /// <summary>
        /// Returns a value indicating whether this method receives the given
        /// <see cref="WampMessage{TMessage}"/> as is.
        /// </summary>
        public bool IsRawMethod => mIsRawMethod;

        /// <summary>
        /// Gets this method's parameters.
        /// </summary>
        public ParameterInfo[] Parameters { get; }

        /// <summary>
        /// Gets this method's parameters that require deserialization.
        /// </summary>
        public ParameterInfo[] ParametersToConvert => mParametersToConvert;

        /// <summary>
        /// Gets the number of arguments of this method.
        /// </summary>
        public int TotalArgumentsCount => mTotalArgumentsCount;

        /// <summary>
        /// Gets the message type this method handles.
        /// </summary>
        public WampMessageType MessageType { get; }

        #endregion
    }
}