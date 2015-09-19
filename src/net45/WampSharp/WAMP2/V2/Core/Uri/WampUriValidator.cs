using System.Text.RegularExpressions;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core
{
    public abstract class WampUriValidator : IWampUriValidator
    {
        public abstract Regex UriPatternAllowEmpty { get; }
        public abstract Regex UriPatternDisallowEmpty { get; }
        public abstract Regex UriPatternAllowLastEmpty { get; }

        public bool IsValid(string uri)
        {
            return UriPatternDisallowEmpty.IsMatch(uri);
        }

        public bool IsValid(string uri, string match)
        {
            Regex regex = GetRegex(match);

            return regex.IsMatch(uri);
        }

        private Regex GetRegex(string match)
        {
            switch (match)
            {
                case WampMatchPattern.Exact:
                    return UriPatternDisallowEmpty;
                case WampMatchPattern.Prefix:
                    return UriPatternAllowLastEmpty;
                case WampMatchPattern.Wildcard:
                    return UriPatternAllowEmpty;
            }

            throw new WampException(WampErrors.InvalidOptions, "unknown match type " + match);
        }
    }
}