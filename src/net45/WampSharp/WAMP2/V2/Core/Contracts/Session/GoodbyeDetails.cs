using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents details of a GOODBYE message.
    /// </summary>
    [DataContract]
    public class GoodbyeDetails : GoodbyeAbortDetails
    {
    }
}