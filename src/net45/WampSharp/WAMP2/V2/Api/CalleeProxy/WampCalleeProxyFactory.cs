#if CASTLE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using WampSharp.Core.Utilities;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeProxyFactory : IWampCalleeProxyFactory
    {
        private readonly ProxyGenerator mGenerator = CastleDynamicProxyGenerator.Instance;
        private readonly WampCalleeProxyInvocationHandler mHandler;

        public WampCalleeProxyFactory(WampCalleeProxyInvocationHandler handler)
        {
            mHandler = handler;
        }

        public virtual TProxy GetProxy<TProxy>(ICalleeProxyInterceptor interceptor) where TProxy : class
        {
            ProxyGenerationOptions options = new ProxyGenerationOptions()
            {
                Selector = new WampCalleeProxyInterceptorSelector()
            };

            IInterceptor[] interceptors =
                GetAllInterfaceMethods(typeof(TProxy))
                    .Select(method => BuildInterceptor(method, interceptor))
                    .ToArray();

            TProxy proxy =
                mGenerator.CreateInterfaceProxyWithoutTarget<TProxy>(options, interceptors);

            return proxy;
        }

        private IInterceptor BuildInterceptor(MethodInfo method, ICalleeProxyInterceptor interceptor)
        {
            return CalleeProxyInterceptorFactory.BuildInterceptor(method, interceptor, mHandler);
        }

        private List<MethodInfo> GetAllInterfaceMethods(Type type)
        {
            List<MethodInfo> methods = new List<MethodInfo>(type.GetMethods());
            foreach (Type interf in type.GetInterfaces())
                foreach (MethodInfo method in interf.GetMethods())
                    if (!methods.Contains(method))
                        methods.Add(method);

            return methods;
        }
    }
}
#endif