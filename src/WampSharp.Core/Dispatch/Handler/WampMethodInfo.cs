using System;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Contracts;
using WampSharp.Core.Contracts.V1;

namespace WampSharp.Core.Dispatch.Handler
{
    public class WampMethodInfo
    {
        private readonly MethodInfo mMethod;
        private readonly int mArgumentsCount;
        private readonly bool mHasWampClientArgument;
        private readonly bool mHasParamsArgument;

        public WampMethodInfo(MethodInfo method)
        {
            mMethod = method;

            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Length > 0)
            {
                if (parameters[0].ParameterType == typeof (IWampClient))
                {
                    mHasWampClientArgument = true;
                }

                if (parameters.Last().IsDefined(typeof (ParamArrayAttribute)))
                {
                    mHasParamsArgument = true;
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
    }
}