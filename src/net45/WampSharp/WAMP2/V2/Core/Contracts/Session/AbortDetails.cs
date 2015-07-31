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

    /// <summary>
    /// Represents details of an ABORT message.
    /// </summary>
    [DataContract]
    public class AbortDetails : GoodbyeAbortDetails
    {
    }


    /// <summary>
    /// Represents details of a GOODBYE message.
    /// </summary>
    [DataContract]
    public class GoodbyeDetails : GoodbyeAbortDetails
    {
    }
}