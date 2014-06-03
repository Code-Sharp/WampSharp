using Castle.DynamicProxy;
using WampSharp.Cra;

namespace WampSharp.Core.Proxy
{
    internal class WampCraAuthenticatorPropertyInterceptor : IInterceptor
    {
        private IWampCraAuthenticator mWampCraAuthenticator;
        
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Arguments.Length > 0)
            {
                mWampCraAuthenticator = invocation.Arguments[0] as IWampCraAuthenticator;
            }
            else
            {
                invocation.ReturnValue = mWampCraAuthenticator;
            }
        }
    }
}