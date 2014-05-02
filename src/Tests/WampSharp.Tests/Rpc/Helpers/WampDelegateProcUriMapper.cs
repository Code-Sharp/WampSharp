using System;
using System.Reflection;
using WampSharp.V1.Rpc.Client;

namespace WampSharp.Tests.Rpc.Helpers
{
    public class WampDelegateProcUriMapper : IWampProcUriMapper
    {
        private readonly Func<MethodInfo, string> mMethodMapper;

        public WampDelegateProcUriMapper(Func<MethodInfo, string> methodMapper)
        {
            mMethodMapper = methodMapper;
        }

        public string Map(MethodInfo method)
        {
            return mMethodMapper(method);
        }
    }
}