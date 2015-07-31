using System.Reflection;
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

            result.ReturnType = 
                TaskExtensions.UnwrapReturnType(method.ReturnType);

            return result;
        }
    }
}