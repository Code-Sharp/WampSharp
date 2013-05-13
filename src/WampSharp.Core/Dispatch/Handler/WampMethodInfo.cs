using System;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

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
        private readonly bool mHasWampClientArgument;
        private readonly bool mHasParamsArgument;
        private readonly bool mIsRawMethod;

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

            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length > 0)
            {
                if (parameters[0].IsDefined(typeof(WampProxyParameterAttribute)))
                {
                    mHasWampClientArgument = true;
                }

                ParameterInfo lastParameter = parameters.Last();

                if (lastParameter.IsDefined(typeof (ParamArrayAttribute)))
                {
                    mHasParamsArgument = true;
                }

                if (typeof (WampMessage<>).IsAssignableFromGeneric(lastParameter.ParameterType))
                {
                    mIsRawMethod = true;
                }
            }

            mArgumentsCount = parameters.Length;

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

        #endregion
    }
}