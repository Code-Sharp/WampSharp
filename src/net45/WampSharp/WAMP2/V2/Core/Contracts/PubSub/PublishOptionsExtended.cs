using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    public class PublishOptionsExtended : PublishOptions
    {
        [IgnoreDataMember]
        public long PublisherId { get; set; }
    }
}