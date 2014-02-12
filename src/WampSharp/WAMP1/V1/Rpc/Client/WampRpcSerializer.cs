using System;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.Core.Utilities;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampRpcSerializer"/>.
    /// </summary>
    public class WampRpcSerializer : IWampRpcSerializer
    {
        private readonly IWampProcUriMapper mProcUriMapper;

        /// <summary>
        /// Creates a new instance of <see cref="WampRpcSerializer"/>.
        /// </summary>
        /// <param name="procUriMapper">A given <see cref="IWampProcUriMapper"/>
        /// used in order to map called methods to their corresponding
        /// uris.</param>
        public WampRpcSerializer(IWampProcUriMapper procUriMapper)
        {
            mProcUriMapper = procUriMapper;
        }

        public WampRpcCall Serialize(MethodInfo method, object[] arguments)
        {
            WampRpcCall result = new WampRpcCall()
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

            Type taskType = 
                returnType.GetClosedGenericTypeImplementation(typeof (Task<>));

            if (taskType != null)
            {
                return returnType.GetGenericArguments()[0];
            }

            return returnType;
        }
    }
}