using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WampSharp.V1.Rpc.Server
{
    /// <summary>
    /// An implementation of <see cref="IWampRpcMetadataCatalog"/>.
    /// </summary>
    public class WampRpcMetadataCatalog : IWampRpcMetadataCatalog
    {
        private readonly ConcurrentDictionary<string, IWampRpcMethod> mProcUriToMethod;

        /// <summary>
        /// A default constructor.
        /// </summary>
        public WampRpcMetadataCatalog()
        {
            mProcUriToMethod =
                new ConcurrentDictionary<string, IWampRpcMethod>
                    (StringComparer.Ordinal);
        }

        public void Register(IWampRpcMetadata metadata)
        {
            IEnumerable<IWampRpcMethod> newMethods = metadata.GetServiceMethods();

            foreach (var procUriToMethod in newMethods)
            {
                bool added = 
                    mProcUriToMethod.TryAdd(procUriToMethod.ProcUri, procUriToMethod);
                
                if (!added)
                {
                    throw new ProcUriAlreadyMappedException(procUriToMethod.ProcUri);
                }
            }
        }
        
        public bool Unregister(IWampRpcMethod method)
        {

            if (mProcUriToMethod.TryRemove(method.ProcUri, out IWampRpcMethod originalMethod))
            {
                return true;
            }

            return false;
        }

        public IWampRpcMethod ResolveMethodByProcUri(string procUri)
        {

            if (mProcUriToMethod.TryGetValue(procUri, out IWampRpcMethod method))
            {
                return method;
            }

            return null;
        }

        public IEnumerable<IWampRpcMethod> GetAllRpcMethods()
        {
            return mProcUriToMethod.Values;
        }
    }
}