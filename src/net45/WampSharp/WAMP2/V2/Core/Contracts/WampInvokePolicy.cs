namespace WampSharp.V2.Core.Contracts
{
    public static class WampInvokePolicy
    {
        public const string Single = "single";
        public const string First = "first";
        public const string Last = "last";
        public const string Random = "random";
        public const string Roundrobin = "roundrobin";
        public const string Default = Single;
    }
}