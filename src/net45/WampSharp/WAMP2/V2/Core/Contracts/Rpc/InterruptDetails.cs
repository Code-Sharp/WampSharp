using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    [WampDetailsOptions(WampMessageType.v2Interrupt)]
    public class InterruptDetails : WampDetailsOptions
    {
        [DataMember(Name = "mode")]
        public string Mode { get; set; }
    }
}