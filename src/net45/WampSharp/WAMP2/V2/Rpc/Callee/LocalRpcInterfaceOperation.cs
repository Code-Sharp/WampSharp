using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using WampSharp.Core.Serialization;
using WampSharp.Logging;
using WampSharp.V2;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Error;
using WampSharp.V2.Rpc;

namespace WampSharp.WAMP2.V2.Rpc.Callee
{
    public class LocalRpcInterfaceOperation: IWampRpcOperation
    {
        private readonly string mPrefix;
        private readonly string mPath;
        private readonly ILog mLogger;
        private readonly Dictionary<string, OperationToRegister> mOperationMap =
          new Dictionary<string, OperationToRegister>();
        protected readonly static IWampFormatter<object> ObjectFormatter = WampObjectFormatter.Value;
        private Func<object> mInstanceProvider;

        public LocalRpcInterfaceOperation(object instance, string prefix) :
            this(instance, String.Empty, prefix, CalleeRegistrationInterceptor.Default)
        {
        }

        public LocalRpcInterfaceOperation(object instance, string prefix, ICalleeRegistrationInterceptor interceptor) : 
            this(instance, String.Empty, prefix, interceptor) {
        }

        public LocalRpcInterfaceOperation(object instance, string path, string prefix, ICalleeRegistrationInterceptor interceptor) : this(instance.GetType(), () => instance, path, prefix, interceptor)
        {

        }

        public LocalRpcInterfaceOperation(Type type, Func<object> instanceProvider, string prefix,
            ICalleeRegistrationInterceptor interceptor)
            : this(type, instanceProvider, String.Empty, prefix, interceptor)
        {
            
        }

        public LocalRpcInterfaceOperation(Type type, Func<object> instanceProvider, string path, string prefix, ICalleeRegistrationInterceptor interceptor)
        {
            mInstanceProvider = instanceProvider;
            mPrefix = prefix;
            mPath = path;
            string fullPath = (String.IsNullOrEmpty(path) ? "" : path + ".") + prefix;
            mLogger = LogProvider.GetLogger(typeof(LocalRpcInterfaceOperation) + "." + fullPath);

            OperationExtractor extractor = new OperationExtractor();

            ContextualizingCalleeRegistrationInterceptor contextualizingCalleeRegistrationInterceptor = new ContextualizingCalleeRegistrationInterceptor(interceptor, fullPath);
            IEnumerable<OperationToRegister> operationsToRegister =
                extractor.ExtractOperations(type, instanceProvider, contextualizingCalleeRegistrationInterceptor);

            foreach (var operationMapRecord in operationsToRegister)
            {
                mOperationMap.Add(operationMapRecord.Operation.Procedure, operationMapRecord);
            }
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
       InvocationDetails details)
        {
            InnerInvoke(caller, formatter, details, null, null);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
            InvocationDetails details, TMessage[] arguments)
        {
            InnerInvoke(caller, formatter, details, arguments, null);
        }

        public void Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter,
            InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            InnerInvoke(caller, formatter, details, arguments, argumentsKeywords);
        }

        protected void InnerInvoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
            IWampFormatter<TMessage> formatter,
            InvocationDetails details,
            TMessage[] arguments,
            IDictionary<string, TMessage> argumentsKeywords)
        {
            IWampRpcOperation operation = FindLongestMatch(details.Procedure);

            if (operation != null)
                operation.Invoke(caller, formatter, details, arguments, argumentsKeywords);
            else
            {
                IWampErrorCallback callback = new WampRpcErrorCallback(caller);

                WampRpcRuntimeException wampException =
                    ConvertExceptionToRuntimeException(new NotSupportedException("Method not supported " + details.Procedure));
                callback.Error(wampException);
            }
        }

        private IWampRpcOperation FindLongestMatch(string procedure)
        {
            while (procedure.Length > mPrefix.Length)
            {
                OperationToRegister record;
                if (mOperationMap.TryGetValue(procedure, out record))
                {
                    return record.Operation;
                }
                procedure = procedure.Substring(0, procedure.LastIndexOf('.'));
            }
            return null;
        }

        protected static WampRpcRuntimeException ConvertExceptionToRuntimeException(Exception exception)
        {
            return new WampRpcRuntimeException(exception.Message, exception);
        }

        protected class WampRpcErrorCallback : IWampErrorCallback
        {
            private readonly IWampRawRpcOperationRouterCallback mCallback;

            public WampRpcErrorCallback(IWampRawRpcOperationRouterCallback callback)
            {
                mCallback = callback;
            }

            public void Error(object details, string error)
            {
                mCallback.Error(ObjectFormatter, details, error);
            }

            public void Error(object details, string error, object[] arguments)
            {
                mCallback.Error(ObjectFormatter, details, error, arguments);
            }

            public void Error(object details, string error, object[] arguments, object argumentsKeywords)
            {
                mCallback.Error(ObjectFormatter, details, error, arguments, argumentsKeywords);
            }
        }
        public string Procedure
        {
            get { return mPrefix; }
        }

        protected class ContextualizingCalleeRegistrationInterceptor: ICalleeRegistrationInterceptor
        {
            private readonly ICalleeRegistrationInterceptor mOuterInterceptor;
            private readonly string mPath;

            public ContextualizingCalleeRegistrationInterceptor(ICalleeRegistrationInterceptor outerInterceptor, string path)
            {
                if (outerInterceptor is ContextualizingCalleeRegistrationInterceptor)
                    mOuterInterceptor =
                        (outerInterceptor as ContextualizingCalleeRegistrationInterceptor).mOuterInterceptor;
                else
                    mOuterInterceptor = outerInterceptor;
                mPath = path;
            }

            public bool IsCalleeMember(MemberInfo member)
            {
                return mOuterInterceptor.IsCalleeMember(member);
            }

            public RegisterOptions GetRegisterOptions(MemberInfo method)
            {
                return mOuterInterceptor.GetRegisterOptions(method);
            }

            public string GetProcedureUri(MemberInfo method)
            {
                return (String.IsNullOrEmpty(mPath) ? "" : mPath + ".") + mOuterInterceptor.GetProcedureUri(method);
            }
        }
    }
}
