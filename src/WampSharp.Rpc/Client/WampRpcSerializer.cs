using System.Reflection;

namespace WampSharp.Rpc
{
    public class WampRpcSerializer : IWampRpcSerializer
    {
        private readonly IProcUriMapper mProcUriMapper;

        public WampRpcSerializer(IProcUriMapper procUriMapper)
        {
            mProcUriMapper = procUriMapper;
        }

        public WampRpcCall<object> Serialize(MethodInfo method, object[] arguments)
        {
            WampRpcCall<object> result = new WampRpcCall<object>()
                                             {
                                                 Arguments =  arguments,
                                                 ProcUri =  mProcUriMapper.Map(method),
                                                 ReturnType = method.ReturnType
                                             };

            return result;
        }
    }
}