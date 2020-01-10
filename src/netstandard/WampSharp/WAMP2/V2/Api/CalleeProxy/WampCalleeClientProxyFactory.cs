using System;
using System.Reflection;
using WampSharp.V2.Client;
using WampSharp.CodeGeneration;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeClientProxyFactory : IWampCalleeProxyFactory
    {
        private readonly IWampRealmProxy mProxy;
        private readonly WampCalleeProxyInvocationHandler mHandler;

        public WampCalleeClientProxyFactory(IWampRealmProxy proxy)
        {
            mProxy = proxy;
            mHandler = new ClientInvocationHandler(proxy);
        }

        public virtual TProxy GetProxy<TProxy>(ICalleeProxyInterceptor interceptor) where TProxy : class
        {
            if (typeof(CalleeProxyBase).IsAssignableFrom(typeof(TProxy)))
            {
                return (TProxy) Activator.CreateInstance(typeof(TProxy), mProxy, interceptor);
            }

            try
            {
                TProxy result = DispatchProxy.Create<TProxy, CalleeDispatchProxy>();

                CalleeDispatchProxy casted = result as CalleeDispatchProxy;

                casted.Handler = mHandler;
                casted.CalleeProxyInterceptor = interceptor;

                return result;
            }
            catch (NotImplementedException)
            {
                Exception exception = CreateExceptionWithGeneratedCode<TProxy>();
                throw exception;
            }
        }

        private static Exception CreateExceptionWithGeneratedCode<T>() where T : class
        {
            CalleeProxyCodeGenerator generator = new CalleeProxyCodeGenerator(typeof (T).Namespace + ".Generated");

            string generatedCode = generator.GenerateCode(typeof(T));

            return new NotSupportedException
                ("No runtime type code generation available on this platform." +
                 " You might want to try using GetProxy with the type declared in the inner exception.",
                 new GeneratedCodeException(generatedCode));
        }
    }
}