using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents details of a GOODBYE message.
    /// </summary>
    [DataContract]
    [Serializable]
    [WampDetailsOptions(WampMessageType.v2Goodbye)]
    public class GoodbyeDetails : GoodbyeAbortDetails
    {
    }
}