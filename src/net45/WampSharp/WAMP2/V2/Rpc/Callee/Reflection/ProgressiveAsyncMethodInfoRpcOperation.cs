using System;
using System.Collections.Generic;
using System.Reflection;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class ProgressiveAsyncMethodInfoRpcOperation<T> : AsyncMethodInfoRpcOperation
    {
        private readonly RpcParameter[] mRpcParameters;

        public ProgressiveAsyncMethodInfoRpcOperation(Func<object> instanceProvider, MethodInfo method, string procedureName) : 
            base(instanceProvider, method, procedureName)
        {
            RpcParameter[] baseParameters = base.Parameters;

            mRpcParameters = new RpcParameter[baseParameters.Length - 1];

            Array.Copy(baseParameters,
                mRpcParameters,
                mRpcParameters.Length);
        }

        protected override object[] GetMethodParameters<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            object[] argumentsWithoutProgress = 
                base.GetMethodParameters(caller, formatter, arguments, argumentsKeywords);

            int length = argumentsWithoutProgress.Length + 1;

            object[] result = new object[length];

            Array.Copy(argumentsWithoutProgress,
                result,
                argumentsWithoutProgress.Length);

            result[length - 1] = new CallerProgress(caller, this);

            return result;
        }

        public override RpcParameter[] Parameters
        {
            get
            {
                return mRpcParameters;
            }
        }

        private class CallerProgress : IProgress<T>
        {
            private readonly IWampRawRpcOperationRouterCallback mCaller;
            private readonly ProgressiveAsyncMethodInfoRpcOperation<T> mParent;

            public CallerProgress(IWampRawRpcOperationRouterCallback caller,
                                  ProgressiveAsyncMethodInfoRpcOperation<T> parent)
            {
                mCaller = caller;
                mParent = parent;
            }

            public void Report(T value)
            {
                mParent.CallResult(mCaller, value, new YieldOptions() {Progress = true});
            }
        }
    }
}