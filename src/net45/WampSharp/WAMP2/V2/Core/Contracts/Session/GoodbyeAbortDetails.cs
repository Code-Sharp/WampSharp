using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public abstract class GoodbyeAbortDetails : WampDetailsOptions
    {
        /// <summary>
        /// The message sent upon the ABORT message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}