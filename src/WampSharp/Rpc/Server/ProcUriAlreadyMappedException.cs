using System;
using System.Runtime.Serialization;

namespace WampSharp.Rpc.Server
{
    /// <summary>
    /// Indicates that a given proc uri is already mapped in
    /// <see cref="IWampRpcMetadataCatalog"/>.
    /// </summary>
    [Serializable]
    public class ProcUriAlreadyMappedException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="ProcUriAlreadyMappedException"/>.
        /// </summary>
        /// <param name="procUri">The given proc uri.</param>
        public ProcUriAlreadyMappedException(string procUri) : base(string.Format("The following procUri is already mapped: {0}", procUri))
        {
        }

        protected ProcUriAlreadyMappedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}