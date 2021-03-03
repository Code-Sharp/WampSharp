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
    public class ProgressiveObservableMethodInfoRpcOperation<T> : SyncMethodInfoRpcOperation
    {
        public ProgressiveObservableMethodInfoRpcOperation(Func<object> instanceProvider, MethodInfo method, string procedureName) :
            base(instanceProvider, method, procedureName)
        {
        }

        protected override IWampCancellableInvocation OnResult(IWampRawRpcOperationRouterCallback caller, object result, IDictionary<string, object> outputs)
        {
            var ctSource = new CancellationTokenSource();
            ((IObservable<T>)result).Subscribe(
                it => CallResult(caller, it, outputs, new YieldOptions { Progress = true }),
                ex =>
                {
                    if (ex is WampException wampex)
                        HandleException(caller, wampex);
                    else
                        HandleException(caller, ex);
                },
                // An observable does not emit any value when completing, so result without arguments
                () => caller.Result(ObjectFormatter, new YieldOptions()),
                ctSource.Token
            );
            return new CancellationTokenSourceInvocation(ctSource);
        }
    }
}
