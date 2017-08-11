namespace WampSharp.V2.Testament
{
    public static class WampTestamentScope
    {
        public const string Detached = "detatched";
        public const string Destroyed = "destroyed";

        public static readonly string[] Scopes = {Destroyed, Detached};
    }
}