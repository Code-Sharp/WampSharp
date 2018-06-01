using System;
using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal interface IOperationExtractor
    {
        IEnumerable<OperationToRegister> ExtractOperations(Type serviceType, Func<object> instance, ICalleeRegistrationInterceptor interceptor);
    }

    internal class OperationToRegister
    {
        private readonly RegisterOptions mOptions;

        public OperationToRegister(IWampRpcOperation operation, RegisterOptions options)
        {
            Operation = operation;
            mOptions = options;
        }

        public IWampRpcOperation Operation { get; }

        public RegisterOptions Options => mOptions;
    }
}