using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class ProgressiveObservableMethodInfoRpcOperation<T> : SyncMethodInfoRpcOperation
    {
        public ProgressiveObservableMethodInfoRpcOperation(Func<object> instanceProvider, MethodInfo method,
                                                           string procedureName) :
            base(instanceProvider, method, procedureName)
        {
        }

        protected override IWampCancellableInvocation MethodInvocation<TMessage>(
            IWampRawRpcOperationRouterCallback caller,
            IWampFormatter<TMessage> formatter,
            InvocationDetails details,
            TMessage[] arguments,
            IDictionary<string, TMessage> argumentsKeywords)
        {
            object methodInfoResult =
                InvokeSync(caller,
                           formatter,
                           details,
                           arguments,
                           argumentsKeywords,
                           out IDictionary<string, object> outputs);

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            IObservable<T> resultAsObservable = (IObservable<T>)methodInfoResult;

            resultAsObservable.Subscribe
                (
                 value =>
                 {
                     IDictionary<string, object> resultArgumentsKeywords = GetResultArgumentKeywords(value, null);
                     CallResult(caller, value, resultArgumentsKeywords, new YieldOptions { Progress = true });
                 },
                 ex =>
                 {
                     if (ex is WampException wampException)
                     {
                         HandleException(caller, wampException);
                     }
                     else
                     {
                         HandleException(caller, ex);
                     }
                 },
                 // An observable does not emit any value when completing, so result without arguments
                 () => caller.Result(ObjectFormatter, new YieldOptions()),
                 tokenSource.Token
                );

            return new CancellationTokenSourceInvocation(tokenSource);
        }
    }
}
