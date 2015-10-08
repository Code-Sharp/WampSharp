using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using WampSharp.Core.Contracts;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    internal class MockClientInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (method.IsDefined(typeof(WampHandlerAttribute), true))
            {
                return interceptors.OfType<RecordAndPlayInterceptor<MockRaw>>()
                    .Cast<IInterceptor>()
                    .ToArray();                
            }
            else if (method.IsDefined(typeof(WampRawHandlerAttribute), true))
            {
                return interceptors.OfType<RecordAndPlayRawInterceptor<MockRaw>>()
                    .Cast<IInterceptor>()
                    .ToArray();
            }
            if (method.Name == "get_Session")
            {
                return interceptors.OfType<SessionPropertyInterceptor>()
                    .Cast<IInterceptor>()
                    .ToArray();
            }
            if (method.Name == "get_WelcomeDetails")
            {
                return interceptors.OfType<WelcomeDetailsInterceptor>()
                    .Cast<IInterceptor>()
                    .ToArray();
            }

            return new IInterceptor[] { new NullInterceptor() };
        }
    }
}