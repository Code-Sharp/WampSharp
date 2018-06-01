using System;
using System.Runtime.Serialization;

namespace WampSharp.V1.Rpc.Server
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
        public ProcUriAlreadyMappedException(string procUri) : base($"The following procUri is already mapped: {procUri}")
        {
        }

        protected ProcUriAlreadyMappedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}