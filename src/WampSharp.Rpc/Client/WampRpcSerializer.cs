using System;
using System.Reflection;
using System.Threading.Tasks;

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
                                             };

            result.ReturnType = ExtractReturnType(method.ReturnType);

            return result;
        }

        private Type ExtractReturnType(Type returnType)
        {
            if (returnType == typeof (void) || returnType == typeof(Task))
            {
                return typeof (object);
            }
            
            if (typeof (Task).IsAssignableFrom(returnType) &&
                returnType.IsGenericType && 
                returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return returnType.GetGenericArguments()[0];
            }

            return returnType;
        }
    }
}