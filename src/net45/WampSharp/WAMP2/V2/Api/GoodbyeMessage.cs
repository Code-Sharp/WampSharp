using System;
using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    public class GoodbyeMessage
    {
        public string Reason { get; internal set; }
        public GoodbyeDetails Details { get; internal set; }
    }
}