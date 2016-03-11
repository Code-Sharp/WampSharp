using System;
using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    public abstract class GoodbyeAbortDetails : WampDetailsOptions
    {
        /// <summary>
        /// The message sent upon the ABORT message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}