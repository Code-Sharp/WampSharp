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

        // We can't use Observable.FromAsync, as there is no overload that receives IObserver, and therefore we can't convert it into
        // an IProgress instance and forward it to as the IProgress argument of InvokeProgressiveAsync.
        // However, Observable.FromAsync and Observable.Create both use TaskObservableExtensions.Subscribe, which
        // eventually uses EmitTaskResult so this is not a big deal.
        public override object Invoke(MethodInfo method, object[] arguments)
        {
            return Observable.Create<T>(async (observer, cancellationToken) =>
                                        {
                                            T last = await Handler.InvokeProgressiveAsync
                                                (Interceptor, method, ResultExtractor, ResultExtractor, arguments, observer.ToProgress(),
                                                 cancellationToken);

                                            if (last != null)
                                            {
                                                observer.OnNext(last);
                                            }

                                            observer.OnCompleted();
                                        });
        }
    }
}
