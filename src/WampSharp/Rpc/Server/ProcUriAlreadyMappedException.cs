using System;
using System.Runtime.Serialization;

namespace WampSharp.Rpc.Server
{
    [Serializable]
    public class ProcUriAlreadyMappedException : Exception
    {
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