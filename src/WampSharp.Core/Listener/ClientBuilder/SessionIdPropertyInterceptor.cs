using Castle.DynamicProxy;

namespace WampSharp.Core.Listener
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