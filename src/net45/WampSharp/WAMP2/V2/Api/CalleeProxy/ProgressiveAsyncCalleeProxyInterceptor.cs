using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using WampSharp.V2.Core.Contracts;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.V2.CalleeProxy
{
    internal class ProgressiveAsyncCalleeProxyInterceptor : IInterceptor
    {
        private static readonly MethodInfo mInvokeProgressiveAsync =
            typeof (IWampCalleeProxyInvocationHandler).GetMethod("InvokeProgressiveAsync");

        private readonly IWampCalleeProxyInvocationHandler mHandler;
        private readonly CallOptions mCallOptions;

        public ProgressiveAsyncCalleeProxyInterceptor(IWampCalleeProxyInvocationHandler handler, CallOptions callOptions)
        {
            mHandler = handler;
            mCallOptions = callOptions;
        }

        public void Intercept(IInvocation invocation)
        {
            CallOptions options = new CallOptions(mCallOptions) {ReceiveProgress = true};

            ParameterInfo lastParameter = invocation.Method.GetParameters().LastOrDefault();

            Type progressType = lastParameter.ParameterType.GetGenericArguments()[0];

            MethodInfo methodToInvoke = 
                mInvokeProgressiveAsync.MakeGenericMethod(progressType);

            object[] arguments = invocation.Arguments;
            
            object[] argumentsWithoutProgress = new object[arguments.Length - 1];

            Array.Copy(arguments, argumentsWithoutProgress, argumentsWithoutProgress.Length);

            Task result =
                (Task) methodToInvoke.Invoke(mHandler,
                    new object[] {options, invocation.Method, argumentsWithoutProgress, arguments.Last()});

            invocation.ReturnValue = result;
        }
    }
}