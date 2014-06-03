using Castle.DynamicProxy;
using WampSharp.V1.Cra;

namespace WampSharp.V1.Core.Proxy
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