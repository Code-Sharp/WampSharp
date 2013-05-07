using System;
using System.Reflection;

namespace WampSharp.Rpc
{
    public class DelegateProcUriMapper : IProcUriMapper
    {
        private readonly Func<MethodInfo, string> mMapper;

        public DelegateProcUriMapper(Func<MethodInfo, string> mapper)
        {
            mMapper = mapper;
        }

        public string Map(MethodInfo method)
        {
            return mMapper(method);
        }
    }
}