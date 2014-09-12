namespace WampSharp.V2.Core.Contracts
{
    internal class PublishOptionsExtended : PublishOptions
    {
        [IgnoreProperty]
        public long PublisherId { get; set; }
    }
}