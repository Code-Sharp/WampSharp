using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using WampSharp.Core.Utilities;
using WampSharp.Core.Serialization;
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
            IEnumerable<object> parameters = UnpackParameters(formatter, arguments, argumentsKeywords);

            CallerProgress progress = new CallerProgress(caller, this);

            parameters = parameters.Concat(progress);

            if (SupportsCancellation)
            {
                parameters = parameters.Concat(cancellationToken);
            }

            object[] result = parameters.ToArray();

            return result;
        }

        public override RpcParameter[] Parameters => mRpcParameters;

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