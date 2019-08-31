using System.Collections.Generic;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.Tests.Rpc.Helpers
{
    public class MockRpcCatalog : IWampRpcMetadataCatalog
    {
        private readonly IDictionary<string, IWampRpcMethod> mProcUriToMethod =
            new Dictionary<string, IWampRpcMethod>();

        public void MapMethod(MockRpcMethod method)
        {
            mProcUriToMethod[method.ProcUri] = method;
        }

        public void Register(IWampRpcMetadata metadata)
        {
        }

        public bool Unregister(IWampRpcMethod method)
        {
            return true;
        }

        public IWampRpcMethod ResolveMethodByProcUri(string procUri)
        {
            return mProcUriToMethod[procUri];
        }
        
        public IEnumerable<IWampRpcMethod> GetAllRpcMethods()
        {
            return mProcUriToMethod.Values;
        }
    }
}