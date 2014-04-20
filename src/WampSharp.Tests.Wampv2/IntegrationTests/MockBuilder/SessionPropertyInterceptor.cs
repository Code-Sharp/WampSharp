using Castle.DynamicProxy;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    internal class SessionPropertyInterceptor : IInterceptor
    {
        private readonly long mSession;

        public SessionPropertyInterceptor(long session)
        {
            mSession = session;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = mSession;
        }
    }
}