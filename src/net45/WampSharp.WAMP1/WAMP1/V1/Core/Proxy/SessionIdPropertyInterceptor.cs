using Castle.DynamicProxy;

namespace WampSharp.V1.Core.Proxy
{
    internal class SessionIdPropertyInterceptor : IInterceptor
    {
        public string SessionId { get; set; }

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = SessionId;
        }
    }
}