using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents details of an ABORT message.
    /// </summary>
    [DataContract]
    public class AbortDetails : WampDetailsOptions
    {
        /// <summary>
        /// The message sent upon the ABORT message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}