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
        private readonly IDictionary<string, IWampRpcMethod> mProcUriToMethod;

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
                try
                {
                    mProcUriToMethod.Add(procUriToMethod.ProcUri, procUriToMethod);
                }
                catch (ArgumentException e)
                {
                    throw new ProcUriAlreadyMappedException(procUriToMethod.ProcUri);
                }
            }
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