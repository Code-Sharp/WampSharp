using System.Text.RegularExpressions;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core
{
    /// <summary>
    /// A base class for regex based <see cref="IWampUriValidator"/>s.
    /// </summary>
    public abstract class WampUriRegexValidator : IWampUriValidator
    {
        /// <summary>
        /// Gets the regex to verify uris that allow empty components.
        /// </summary>
        public abstract Regex UriPatternAllowEmpty { get; }

        /// <summary>
        /// Gets the regex to verify uris that disallow empty components.
        /// </summary>
        public abstract Regex UriPatternDisallowEmpty { get; }

        /// <summary>
        /// Gets the regex to verify uris that allow only the last component to be empty.
        /// </summary>
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