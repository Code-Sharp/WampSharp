using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    [WampDetailsOptions(WampMessageType.v2Result)]
    public class ResultDetails : WampDetailsOptions
    {
        /// <summary>
        /// If <see langword="true"/>, this result is a progressive call result, and subsequent results (or a final error) will follow.
        /// </summary>
        [DataMember(Name = "progress")]
        public bool? Progress { get; internal set; }
    }
}