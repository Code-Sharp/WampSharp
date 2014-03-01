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

        private readonly MethodInfo mMethod;
        private readonly int mArgumentsCount;
        private readonly int mTotalArgumentsCount;
        private readonly bool mHasWampClientArgument;
        private readonly bool mHasParamsArgument;
        private readonly bool mIsRawMethod;
        private readonly ParameterInfo[] mParameters;
        private readonly ParameterInfo[] mParametersToConvert;
        private readonly WampMessageType mMessageType;

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
            mMethod = method;

            WampHandlerAttribute handlerAttribute = 
                method.GetCustomAttribute<WampHandlerAttribute>();

            if (handlerAttribute != null)
            {
                mMessageType = handlerAttribute.MessageType;
            }
            else
            {
                mMessageType = WampMessageType.Unknown;
            }

            ParameterInfo[] parameters = method.GetParameters();

            mParameters = parameters;
            List<ParameterInfo> parametersToConvert = mParameters.ToList();

            if (parameters.Length > 0)
            {
                if (parameters[0].IsDefined(typeof(WampProxyParameterAttribute), true))
                {
                    mHasWampClientArgument = true;
                    parametersToConvert.RemoveAt(0);
                }

                ParameterInfo lastParameter = parameters.Last();

                if (lastParameter.IsDefined(typeof(ParamArrayAttribute), true))
                {
                    mHasParamsArgument = true;
                }

                if (typeof (WampMessage<>).IsGenericAssignableFrom(lastParameter.ParameterType))
                {
                    mIsRawMethod = true;
                    parametersToConvert.RemoveAt(parametersToConvert.Count - 1);
                }
            }

            mTotalArgumentsCount = parameters.Length;
            mArgumentsCount = mTotalArgumentsCount;
            mParametersToConvert = parametersToConvert.ToArray();

            if (mHasWampClientArgument)
            {
                mArgumentsCount = mArgumentsCount - 1;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> this method represents.
        /// </summary>
        public MethodInfo Method
        {
            get
            {
                return mMethod;
            }
        }

        /// <summary>
        /// Gets the number of arguments this method gets, not including
        /// <see cref="WampProxyParameterAttribute"/> parameter.
        /// </summary>
        public int ArgumentsCount
        {
            get
            {
                return mArgumentsCount;
            }
        }

        /// <summary>
        /// Returns a value indicating whether this method gets a
        /// <see cref="WampProxyParameterAttribute"/> parameter.
        /// </summary>
        public bool HasWampClientArgument
        {
            get
            {
                return mHasWampClientArgument;
            }
        }

        /// <summary>
        /// Returns a value indicating whether this method has a params
        /// argument.
        /// </summary>
        public bool HasParamsArgument
        {
            get
            {
                return mHasParamsArgument;
            }
        }

        /// <summary>
        /// Returns a value indicating whether this method receives the given
        /// <see cref="WampMessage{TMessage}"/> as is.
        /// </summary>
        public bool IsRawMethod
        {
            get
            {
                return mIsRawMethod;
            }
        }

        public ParameterInfo[] Parameters
        {
            get
            {
                return mParameters;
            }
        }

        public ParameterInfo[] ParametersToConvert
        {
            get
            {
                return mParametersToConvert;
            }
        }

        public int TotalArgumentsCount
        {
            get { return mTotalArgumentsCount; }
        }

        public WampMessageType MessageType
        {
            get
            {
                return mMessageType;
            }
        }

        #endregion
   }
}