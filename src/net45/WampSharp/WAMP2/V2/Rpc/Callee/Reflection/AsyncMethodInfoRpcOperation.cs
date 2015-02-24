using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class AsyncMethodInfoRpcOperation : AsyncLocalRpcOperation
    {
        private readonly object mInstance;
        private readonly MethodInfo mMethod;
        private readonly RpcParameter[] mParameters;
        private readonly bool mHasResult;
        private readonly CollectionResultTreatment mCollectionResultTreatment;

        public AsyncMethodInfoRpcOperation(object instance, MethodInfo method, string procedureName) :
            base(procedureName)
        {
            mInstance = instance;
            mMethod = method;

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

            mParameters =
                method.GetParameters()
                      .Select(parameter => new RpcParameter(parameter))
                      .ToArray();
        }


        public override RpcParameter[] Parameters
        {
            get { return mParameters; }
        }

        public override bool HasResult
        {
            get { return mHasResult; }
        }

        public override CollectionResultTreatment CollectionResultTreatment
        {
            get { return mCollectionResultTreatment; }
        }

        protected virtual object[] GetMethodParameters<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            object[] result = UnpackParameters(formatter, arguments, argumentsKeywords);

            return result;
        }

        protected override Task<object> InvokeAsync<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            WampInvocationContext.Current = new WampInvocationContext(details);

            try
            {
                object[] unpacked =
                    GetMethodParameters(caller, formatter, arguments, argumentsKeywords);

                Task result =
                    mMethod.Invoke(mInstance, unpacked) as Task;

                Task<object> casted = result.CastTask();

                return casted;
            }
            catch (TargetInvocationException ex)
            {
                Exception actual = ex.InnerException;

                if (actual is WampException)
                {
                    throw actual;
                }
                else
                {
                    throw ConvertExceptionToRuntimeException(actual);
                }
            }
            finally
            {
                WampInvocationContext.Current = null;
            }
        }

        protected bool Equals(AsyncMethodInfoRpcOperation other)
        {
            return Equals(mInstance, other.mInstance) && Equals(mMethod, other.mMethod) && string.Equals(Procedure, other.Procedure);
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
                var hashCode = (mInstance != null ? mInstance.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (mMethod != null ? mMethod.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Procedure != null ? Procedure.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}