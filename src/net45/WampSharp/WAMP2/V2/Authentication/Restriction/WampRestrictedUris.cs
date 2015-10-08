using System;

namespace WampSharp.V2.Authentication
{
    internal static class WampRestrictedUris
    {
        private static readonly string RestrictedPrefix = "wamp.";

        public static bool IsRestrictedUri(string procedure)
        {
            return procedure.StartsWith(RestrictedPrefix,
                                        StringComparison.Ordinal);
        }
    }
}