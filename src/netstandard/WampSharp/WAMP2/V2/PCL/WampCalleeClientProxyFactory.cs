﻿#if MANUAL_PROXY && !DISPATCH_PROXY
using System;
using System.Reflection;
using WampSharp.CodeGeneration;
using WampSharp.V2.Client;

namespace WampSharp.V2.CalleeProxy
{
    public class WampCalleeClientProxyFactory : IWampCalleeProxyFactory
    {
        private readonly IWampRealmProxy mRealmProxy;

        public WampCalleeClientProxyFactory(IWampRealmProxy realmProxy)
        {
            mRealmProxy = realmProxy;
        }

        public T GetProxy<T>(ICalleeProxyInterceptor interceptor) where T : class
        {
            if (typeof (T).IsInterface())
            {
                GenerateCodeAndThrowException<T>();
            }

            return (T) Activator.CreateInstance(typeof (T), mRealmProxy, interceptor);
        }

        private static void GenerateCodeAndThrowException<T>() where T : class
        {
            CalleeProxyCodeGenerator generator = new CalleeProxyCodeGenerator(typeof (T).Namespace + ".Generated");

            string generatedCode = generator.GenerateCode(typeof(T));

            throw new NotSupportedException
                ("No runtime type code generation available on this platform." +
                 " You might want to try to use this method using the type declared in the inner exception.",
                 new GeneratedCodeException(generatedCode));
        }
    }
}
#endif