using System.Reflection;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    public class CachedCalleeProxyInterceptor : ICalleeProxyInterceptor
    {
        private readonly ICalleeProxyInterceptor mUnderlying;
        
        private readonly SwapDictionary<MethodInfo, CallOptions> mMethodToCallOptions =
            new SwapDictionary<MethodInfo, CallOptions>();

        private readonly SwapDictionary<MethodInfo, string> mMethodToProcedureUri =
            new SwapDictionary<MethodInfo, string>(); 

        public CachedCalleeProxyInterceptor(ICalleeProxyInterceptor underlying)
        {
            mUnderlying = underlying;
        }

        public CallOptions GetCallOptions(MethodInfo method)
        {
            CallOptions result;

            if (!mMethodToCallOptions.TryGetValue(method, out result))
            {
                result = mUnderlying.GetCallOptions(method);
                mMethodToCallOptions[method] = result;
            }

            return result;
        }

        public string GetProcedureUri(MethodInfo method)
        {
            string result;

            if (!mMethodToProcedureUri.TryGetValue(method, out result))
            {
                result = mUnderlying.GetProcedureUri(method);
                mMethodToProcedureUri[method] = result;
            }

            return result;
        }
    }
}