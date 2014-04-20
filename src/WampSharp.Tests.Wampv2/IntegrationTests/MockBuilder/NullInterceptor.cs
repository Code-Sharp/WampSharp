using Castle.DynamicProxy;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public class NullInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
        }
    }
}