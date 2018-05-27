using Castle.DynamicProxy;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    internal class WelcomeDetailsInterceptor : IInterceptor
    {
        private readonly WelcomeDetails mWelcomeDetails;

        public WelcomeDetailsInterceptor(WelcomeDetails welcomeDetails)
        {
            mWelcomeDetails = welcomeDetails;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = mWelcomeDetails;
        }
    }
}