#if !NET40
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace WampSharp.V2.CalleeProxy
{
    internal class ProgressiveAsyncCalleeProxyInterceptor<T> : CalleeProxyInterceptorBase<T>
    {
        public ProgressiveAsyncCalleeProxyInterceptor(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor) : base(method, handler, interceptor)
        {
        }

        public override void Intercept(IInvocation invocation)
        {
            object[] arguments = invocation.Arguments;
            
            object[] argumentsWithoutProgress = new object[arguments.Length - 1];

            Array.Copy(arguments, argumentsWithoutProgress, argumentsWithoutProgress.Length);

            IProgress<T> progress = arguments.Last() as IProgress<T>;

            MethodInfo method = invocation.Method;
            Task result =
                Handler.InvokeProgressiveAsync
                    (Interceptor, method, Extractor, argumentsWithoutProgress, progress);

            invocation.ReturnValue = result;
        }
    }
}
#endif