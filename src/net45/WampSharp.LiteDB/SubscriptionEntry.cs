namespace WampSharp.LiteDB
{
    internal class SubscriptionEntry
    {
        public int Id { get; set; }
        public string Match { get; set; }
        public string Topic { get; set; }
    }
}