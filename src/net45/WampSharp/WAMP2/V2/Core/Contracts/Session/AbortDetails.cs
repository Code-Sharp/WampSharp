using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents details of an ABORT message.
    /// </summary>
    [DataContract]
    public class AbortDetails : GoodbyeAbortDetails
    {
    }
}