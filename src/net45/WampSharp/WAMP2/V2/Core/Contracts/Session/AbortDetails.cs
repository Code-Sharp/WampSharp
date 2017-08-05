using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents details of an ABORT message.
    /// </summary>
    [DataContract]
    [Serializable]
    [WampDetailsOptions(WampMessageType.v2Abort)]
    public class AbortDetails : GoodbyeAbortDetails
    {
    }
}