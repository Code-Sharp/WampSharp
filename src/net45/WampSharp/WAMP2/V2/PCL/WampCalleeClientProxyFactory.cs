#if PCL
using System;
using System.Reflection;
using WampSharp.CodeGeneration;
using WampSharp.V2.Client;

namespace WampSharp.V2.CalleeProxy
{
    public class WampCalleeClientProxyFactory : IWampCalleeProxyFactory
    {
        private readonly IWampRpcOperationCatalogProxy mRpcCatalog;
        private readonly IWampClientConnectionMonitor mMonitor;

        public WampCalleeClientProxyFactory(IWampRpcOperationCatalogProxy rpcCatalog,
                                            IWampClientConnectionMonitor monitor)
        {
            mRpcCatalog = rpcCatalog;
            mMonitor = monitor;
        }

        public T GetProxy<T>(ICalleeProxyInterceptor interceptor) where T : class
        {
            if (typeof (T).IsInterface())
            {
                GenerateCodeAndThrowException<T>();
            }

            return (T) Activator.CreateInstance(typeof (T), mRpcCatalog, mMonitor, interceptor);
        }

        private static void GenerateCodeAndThrowException<T>() where T : class
        {
            const string runtimeGenerationNotSupported =
                "No runtime type code generation available on this platform.";

            CalleeProxyCodeGenerator generator = new CalleeProxyCodeGenerator(typeof (T).Namespace + ".Generated");

            string generatedCode;

            try
            {
                generatedCode = generator.GenerateCode(typeof (T));
            }
            catch (Exception)
            {
                throw new NotSupportedException(runtimeGenerationNotSupported);
            }

            throw new NotSupportedException
                (runtimeGenerationNotSupported +
                 " You might want to try to use this method using the type declared in the inner exception.",
                 new GeneratedCodeException(generatedCode));
        }

        internal class GeneratedCodeException : Exception
        {
            private readonly string mGeneratedCode;

            public GeneratedCodeException(string generatedCode)
                : base("Try the code attached in the GeneratedCode property.")
            {
                mGeneratedCode = generatedCode;
            }

            public string GeneratedCode
            {
                get { return mGeneratedCode; }
            }
        }
    }

}
#endif