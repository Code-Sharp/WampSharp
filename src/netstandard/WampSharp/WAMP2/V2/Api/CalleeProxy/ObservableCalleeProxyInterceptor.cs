using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;

namespace WampSharp.V2.CalleeProxy
{
    internal class ObservableCalleeProxyInterceptor<T> : CalleeProxyInterceptorBase<T>
    {
        public ObservableCalleeProxyInterceptor(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor) : base(method, handler, interceptor)
        {
        }

        public override object Invoke(MethodInfo method, object[] arguments) => Observable.Create<T>(async (obs, cancellationToken) =>
        {
            var last = await Handler.InvokeProgressiveAsync
                (Interceptor, method, Extractor, arguments, obs.ToProgress(), cancellationToken);
            if (last != null)
                obs.OnNext(last);
            obs.OnCompleted();
        });
    }
}
