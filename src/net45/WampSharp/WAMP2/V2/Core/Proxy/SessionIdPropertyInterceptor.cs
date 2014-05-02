using Castle.DynamicProxy;

namespace WampSharp.V2.Core.Proxy
{
    internal class SessionIdPropertyInterceptor : IInterceptor
    {
        private readonly long mSessionId;

        public SessionIdPropertyInterceptor(long sessionId)
        {
            mSessionId = sessionId;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = mSessionId;
        }
    }
}