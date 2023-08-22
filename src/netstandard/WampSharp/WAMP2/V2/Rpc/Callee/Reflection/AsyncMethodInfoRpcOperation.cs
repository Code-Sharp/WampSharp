using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.Core.Utilities.ValueTuple;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class AsyncMethodInfoRpcOperation : AsyncLocalRpcOperation
    {
        private readonly Func<object> mInstanceProvider;
        private readonly MethodInfo mMethod;
        private readonly Func<object, object[], Task> mMethodInvoker; 
        private readonly RpcParameter[] mParameters;
        private readonly bool mHasResult;
        private readonly CollectionResultTreatment mCollectionResultTreatment;
        private readonly bool mSupportsCancellation;
        private readonly IWampResultExtractor mResultExtractor;

        public AsyncMethodInfoRpcOperation(Func<object> instanceProvider, MethodInfo method, string procedureName) :
            base(procedureName)
        {
            mInstanceProvider = instanceProvider;
            mMethod = method;
            mMethodInvoker = MethodInvokeGenerator.CreateTaskInvokeMethod(method);

            if (method.ReturnType != typeof (Task))
            {
                mHasResult = true;
            }
            else
            {
                mHasResult = false;
            }

            mCollectionResultTreatment = 
                method.GetCollectionResultTreatment();

            IEnumerable<ParameterInfo> parameterInfos = method.GetParameters();

            if (parameterInfos.LastOrDefault()?.ParameterType == typeof(CancellationToken))
            {
                mSupportsCancellation = true;
                parameterInfos = parameterInfos.Take(parameterInfos.Count() - 1);
            }

            mParameters =
                parameterInfos
                      .Select(parameter => new RpcParameter(parameter))
                      .ToArray();

            mResultExtractor = WampResultExtractor.GetResultExtractor(this);

            if (method.ReturnsTuple())
            {
                mResultExtractor = WampResultExtractor.GetValueTupleResultExtractor(method);
            }
        }

        public override RpcParameter[] Parameters => mParameters;

        public override bool HasResult => mHasResult;

        public override bool SupportsCancellation => mSupportsCancellation;

        public override CollectionResultTreatment CollectionResultTreatment => mCollectionResultTreatment;

        protected virtual object[] GetMethodParameters<TMessage>(IWampRawRpcOperationRouterCallback caller, CancellationToken cancellationToken, IWampFormatter<TMessage> formatter, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            IEnumerable<object> parameters = UnpackParameters(formatter, arguments, argumentsKeywords);

            if (SupportsCancellation)
            {
                parameters = parameters.Concat(cancellationToken);
            }

            object[] result = parameters.ToArray();

            return result;
        }

        protected override Task<object> InvokeAsync<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords, CancellationToken cancellationToken)
        {
            WampInvocationContext.Current = new WampInvocationContext(details);

            try
            {
                object[] unpacked =
                    GetMethodParameters(caller, cancellationToken, formatter, arguments, argumentsKeywords);

                object instance = mInstanceProvider();

                ValidateInstanceType(instance, mMethod);

                Task result =
                    mMethodInvoker(instance, unpacked);

                Task<object> casted = result as Task<object>;

                return casted;
            }
            finally
            {
                WampInvocationContext.Current = null;
            }
        }

        protected override object[] GetResultArguments(object result, YieldOptions yieldOptions)
        {
            return mResultExtractor.GetArguments(result);
        }

        protected override IDictionary<string, object> GetResultArgumentKeywords(
            object result, YieldOptions yieldOptions)
        {
            return mResultExtractor.GetArgumentKeywords(result);
        }

        protected bool Equals(AsyncMethodInfoRpcOperation other)
        {
            return Equals(mInstanceProvider, other.mInstanceProvider) && Equals(mMethod, other.mMethod) && string.Equals(Procedure, other.Procedure);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AsyncMethodInfoRpcOperation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (mInstanceProvider != null ? mInstanceProvider.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (mMethod != null ? mMethod.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Procedure != null ? Procedure.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}