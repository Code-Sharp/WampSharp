using Castle.DynamicProxy;

namespace WampSharp.V1.Core.Proxy
{
    internal class SessionIdPropertyInterceptor : IInterceptor
    {
        private readonly string mSessionId;

        public SessionIdPropertyInterceptor(string sessionId)
        {
            mSessionId = sessionId;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = mSessionId;
        }
    }
}