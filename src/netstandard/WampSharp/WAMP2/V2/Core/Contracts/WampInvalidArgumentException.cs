using System;

namespace WampSharp.V2.Core.Contracts
{
    [Serializable]
    public class WampInvalidArgumentException : WampException
    {
        public WampInvalidArgumentException(string details, Exception innerException) :
            base(null,
                 WampErrors.InvalidArgument,
                 new object[] {details},
                 null,
                 "Unable to deserialize a given argument. " + details,
                 innerException)
        {
        }
    }
}