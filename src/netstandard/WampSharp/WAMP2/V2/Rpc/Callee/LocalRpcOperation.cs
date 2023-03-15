using System;
using System.Collections.Generic;
using System.Reflection;
using WampSharp.Logging;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Error;

namespace WampSharp.V2.Rpc
{
    public abstract class LocalRpcOperation : IWampRpcOperation
    {
        protected readonly ILog mLogger;

        protected static readonly IWampFormatter<object> ObjectFormatter =
            WampObjectFormatter.Value;

        protected LocalRpcOperation(string procedure)
        {
            Procedure = procedure;
            mLogger = LogProvider.GetLogger(typeof (LocalRpcOperation) + "." + procedure);
        }

        public string Procedure { get; }

        public abstract RpcParameter[] Parameters
        {
            get;
        }

        public abstract bool HasResult
        {
            get;
        }

        public abstract bool SupportsCancellation
        {
            get;
        }

        /// <summary>
        /// Returns a value indicating whether to treat an ICollection{T} result
        /// as the arguments yield argument. (If false, treats an ICollection{T} result
        /// value as a single argument).
        /// </summary>
        public abstract CollectionResultTreatment CollectionResultTreatment
        {
            get;
        }

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details)
        {
            return InnerInvoke(caller, formatter, details, null, null);
        }

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments)
        {
            return InnerInvoke(caller, formatter, details, arguments, null);
        }

        public IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            return InnerInvoke(caller, formatter, details, arguments, argumentsKeywords);
        }

        protected virtual object[] GetResultArguments(object result)
        {
            IWampResultExtractor extractor = WampResultExtractor.GetResultExtractor(this);

            return extractor.GetArguments(result);
        }

        protected void CallResult(IWampRawRpcOperationRouterCallback caller, YieldOptions options, object[] arguments, IDictionary<string, object> argumentKeywords)
        {
            if (argumentKeywords != null)
            {
                caller.Result(ObjectFormatter, options, arguments, argumentKeywords);
            }
            else if (!this.HasResult || arguments == null)
            {
                caller.Result(ObjectFormatter, options);
            }
            else
            {
                caller.Result(ObjectFormatter, options, arguments);
            }
        }

        protected IEnumerable<object> UnpackParameters<TMessage>(IWampFormatter<TMessage> formatter,
                                                      TMessage[] arguments,
                                                      IDictionary<string, TMessage> argumentsKeywords)
        {
            ArgumentUnpacker unpacker = new ArgumentUnpacker(Parameters);

            IEnumerable<object> result =
                unpacker.UnpackParameters(formatter, arguments, argumentsKeywords);

            return result;
        }

        protected abstract IWampCancellableInvocation InnerInvoke<TMessage>
            (IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);


        protected void ValidateInstanceType(object instance, MethodInfo method)
        {
            Type declaringType = method.DeclaringType;

            if (!declaringType.IsInstanceOfType(instance))
            {
                throw new ArgumentException("Expected an instance of type " + declaringType);
            }
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

        protected static WampException ConvertExceptionToRuntimeException(Exception exception)
        {

            if (exception is OperationCanceledException canceledException)
            {
                return new WampRpcCanceledException(canceledException.Message);
            }

            // TODO: Maybe try a different implementation.
            return new WampRpcRuntimeException(exception.Message);
        }
    }
}
