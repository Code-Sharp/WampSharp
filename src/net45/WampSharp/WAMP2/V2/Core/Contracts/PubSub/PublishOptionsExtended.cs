using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    internal class PublishOptionsExtended : PublishOptions
    {
        [IgnoreDataMember]
        public long PublisherId { get; set; }
    }
}