using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class SyncMethodInfoRpcOperation : SyncLocalRpcOperation
    {
        private readonly object mInstance;
        private readonly MethodInfo mMethod;
        private readonly Func<object, object[], object> mMethodInvoker;
        private readonly MethodInfoHelper mHelper;
        private readonly RpcParameter[] mParameters;
        private readonly bool mHasResult;
        private readonly CollectionResultTreatment mCollectionResultTreatment;

        public SyncMethodInfoRpcOperation(object instance, MethodInfo method, string procedureName) :
            base(procedureName)
        {
            mInstance = instance;
            mMethod = method;
            mMethodInvoker = MethodInvokeGenerator.CreateInvokeMethod(method);

            if (method.ReturnType != typeof (void))
            {
                mHasResult = true;
            }
            else
            {
                mHasResult = false;
            }

            mCollectionResultTreatment =
                method.GetCollectionResultTreatment();

            mHelper = new MethodInfoHelper(method);

            mParameters =
                method.GetParameters()
                    .Where(x => !x.IsOut)
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

        protected override object InvokeSync<TMessage>
            (IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details,
                TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords,
                out IDictionary<string, object> outputs)
        {
            WampInvocationContext.Current = new WampInvocationContext(details);

            try
            {
                object[] unpacked =
                    UnpackParameters(formatter, arguments, argumentsKeywords);

                object[] parameters =
                    mHelper.GetArguments(unpacked);

                object result =
                    mMethodInvoker(mInstance, parameters);

                outputs = mHelper.GetOutOrRefValues(parameters);

                return result;
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

        protected bool Equals(SyncMethodInfoRpcOperation other)
        {
            return Equals(mInstance, other.mInstance) && Equals(mMethod, other.mMethod) &&
                   string.Equals(Procedure, other.Procedure);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SyncMethodInfoRpcOperation) obj);
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