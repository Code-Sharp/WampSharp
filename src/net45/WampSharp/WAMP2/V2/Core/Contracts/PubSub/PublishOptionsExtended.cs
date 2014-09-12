namespace WampSharp.V2.Core.Contracts
{
    public class PublishOptionsExtended : PublishOptions
    {
        [IgnoreProperty]
        public long PublisherId { get; set; }
    }
}