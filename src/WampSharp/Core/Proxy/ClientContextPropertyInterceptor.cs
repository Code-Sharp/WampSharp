using System.Collections.Concurrent;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace WampSharp.Core.Proxy
{
    internal class ClientContextPropertyInterceptor : IInterceptor
    {
        private readonly IDictionary<string, object> mContext =
            new ConcurrentDictionary<string, object>();
        
        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = mContext;
        }
    }
}