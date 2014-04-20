using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using WampSharp.Core.Contracts;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2.MockBuilder
{
    internal class MockClientInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (method.IsDefined(typeof (WampHandlerAttribute)))
            {
                return interceptors.OfType<RecordAndPlayInterceptor<MockRaw>>().ToArray();                
            }
            if (method.Name == "get_Session")
            {
                return interceptors.OfType<SessionPropertyInterceptor>().ToArray();
            }

            return new IInterceptor[] { new NullInterceptor() };
        }
    }
}