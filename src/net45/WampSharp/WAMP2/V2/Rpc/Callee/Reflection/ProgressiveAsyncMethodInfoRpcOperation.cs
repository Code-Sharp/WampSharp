using System;
using System.Collections.Generic;
using System.Reflection;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.Rpc
{
    public class ProgressiveAsyncMethodInfoRpcOperation<T> : AsyncMethodInfoRpcOperation
    {
        private readonly RpcParameter[] mRpcParameters;

        public ProgressiveAsyncMethodInfoRpcOperation(object instance, MethodInfo method, string procedureName) : 
            base(instance, method, procedureName)
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

            result[length - 1] = new CallerProgress<T>(caller);

            return result;
        }

        public override RpcParameter[] Parameters
        {
            get
            {
                return mRpcParameters;
            }
        }
    }
}