using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.Core.Utilities.ValueTuple;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class ProgressiveAsyncMethodInfoRpcOperation<TProgress, TResult> : AsyncMethodInfoRpcOperation
    {
        private readonly RpcParameter[] mRpcParameters;
        private readonly IWampResultExtractor mProgressExtractor;

        public ProgressiveAsyncMethodInfoRpcOperation(Func<object> instanceProvider, MethodInfo method, string procedureName) :
            base(instanceProvider, method, procedureName)
        {
            RpcParameter[] baseParameters = base.Parameters;

            mRpcParameters = new RpcParameter[baseParameters.Length - 1];

            Array.Copy(baseParameters,
                mRpcParameters,
                mRpcParameters.Length);

            mProgressExtractor = WampResultExtractor.GetResultExtractor(this, true);

            ParameterInfo progressParameter = method.GetProgressParameter();

            Type progressType = progressParameter.ParameterType.GetGenericArguments()[0];
            
            if (progressType.IsValueTuple())
            {
                mProgressExtractor = WampResultExtractor.GetValueTupleResultExtractor(progressType, progressParameter);
            }
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

        protected override object[] GetResultArguments(object result, YieldOptions yieldOptions)
        {
            if (yieldOptions.Progress == true)
            {
                return mProgressExtractor.GetArguments(result);
            }
            else
            {
                return base.GetResultArguments(result, yieldOptions);
            }
        }

        protected override IDictionary<string, object> GetResultArgumentKeywords(object result, YieldOptions yieldOptions)
        {
            if (yieldOptions.Progress == true)
            {
                return mProgressExtractor.GetArgumentKeywords(result);
            }
            else
            {
                return base.GetResultArgumentKeywords(result, yieldOptions);
            }
        }

        public override RpcParameter[] Parameters => mRpcParameters;

        private class CallerProgress : IProgress<TProgress>
        {
            private readonly IWampRawRpcOperationRouterCallback mCaller;
            private readonly ProgressiveAsyncMethodInfoRpcOperation<TProgress, TResult> mParent;

            public CallerProgress(IWampRawRpcOperationRouterCallback caller,
                                  ProgressiveAsyncMethodInfoRpcOperation<TProgress, TResult> parent)
            {
                mCaller = caller;
                mParent = parent;
            }

            public void Report(TProgress value)
            {
                mParent.CallResult(mCaller, value, new YieldOptions() { Progress = true });
            }
        }
    }
}