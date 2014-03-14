using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WampSharp.Rpc.Server
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
                    (StringComparer.InvariantCultureIgnoreCase);
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
            IWampRpcMethod originalMethod;

            if (mProcUriToMethod.TryRemove(method.ProcUri, out originalMethod))
            {
                return true;
            }

            return false;
        }

        public IWampRpcMethod ResolveMethodByProcUri(string procUri)
        {
            IWampRpcMethod method;
            
            if (mProcUriToMethod.TryGetValue(procUri, out method))
            {
                return method;
            }

            return null;
        }
    }
}