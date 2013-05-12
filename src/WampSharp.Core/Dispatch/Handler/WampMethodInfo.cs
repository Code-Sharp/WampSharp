using System;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.Core.Dispatch.Handler
{
    public class WampMethodInfo
    {
        private readonly MethodInfo mMethod;
        private readonly int mArgumentsCount;
        private readonly bool mHasWampClientArgument;
        private readonly bool mHasParamsArgument;
        private readonly bool mIsRawMethod;

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

        public MethodInfo Method
        {
            get
            {
                return mMethod;
            }
        }

        public int ArgumentsCount
        {
            get
            {
                return mArgumentsCount;
            }
        }

        public bool HasWampClientArgument
        {
            get
            {
                return mHasWampClientArgument;
            }
        }

        public bool HasParamsArgument
        {
            get
            {
                return mHasParamsArgument;
            }
        }

        public bool IsRawMethod
        {
            get
            {
                return mIsRawMethod;
            }
        }
    }
}