using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    [WampDetailsOptions(WampMessageType.v2Yield)]
    public class YieldOptions : WampDetailsOptions
    {
        /// <summary>
        /// If <see langword="true"/>, this result is a progressive invocation result, and subsequent results (or a final error) will follow.
        /// </summary>
        [DataMember(Name = "progress")]
        public bool? Progress { get; set; }
    }
}