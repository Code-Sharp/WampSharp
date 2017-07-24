using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
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

        protected override object[] GetMethodParameters<TMessage>(IWampRawRpcOperationRouterCallback caller, CancellationToken cancellationToken, IWampFormatter<TMessage> formatter, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            object[] result = UnpackParameters(formatter, arguments, argumentsKeywords);

            int length = result.Length + 1;
            int progressPosition = length - 1;
            int? cancellationTokenPosition = null;

            if (SupportsCancellation)
            {
                length = length + 1;
                cancellationTokenPosition = length - 1;
            }

            object[] resultWithProgress = new object[length];
            result.CopyTo(resultWithProgress, 0);
            result = resultWithProgress;
            result[progressPosition] = new CallerProgress(caller, this);

            if (cancellationTokenPosition != null)
            {
                result[cancellationTokenPosition.Value] = cancellationToken;
            }

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