using System.Text.RegularExpressions;

namespace WampSharp.V2.Core
{
    /// <summary>
    /// A <see cref="IWampUriValidator"/> that validates uris 
    /// via the strict uri definition.
    /// </summary>
    public class StrictUriValidator : WampUriRegexValidator
    {
        /// <summary>
        /// Strict URI check allowing empty URI components
        /// </summary>
        private readonly Regex mUriPatternAllowEmpty;

        /// <summary>
        /// Strict URI check disallowing empty URI components
        /// </summary>
        private readonly Regex mUriPatternDisallowEmpty;

        /// <summary>
        /// Strict URI check disallowing empty URI components in all but the last component
        /// </summary>
        private readonly Regex mUriPatternAllowLastEmpty;

        /// <summary>
        /// Instantiates a new instance of the <see cref="StrictUriValidator"/> class.
        /// </summary>
        public StrictUriValidator()
        {
            RegexOptions regexOptions = RegexOptions.Compiled;

            mUriPatternAllowEmpty = new Regex(@"^(([0-9a-z_]+\.)|\.)*([0-9a-z_]+)?$", regexOptions);
            mUriPatternDisallowEmpty = new Regex(@"^([0-9a-z_]+\.)*([0-9a-z_]+)$", regexOptions);
            mUriPatternAllowLastEmpty = new Regex(@"^([0-9a-z_]+\.)*([0-9a-z_]*)$", regexOptions);
        }

        public override Regex UriPatternAllowEmpty => mUriPatternAllowEmpty;

        public override Regex UriPatternDisallowEmpty => mUriPatternDisallowEmpty;

        public override Regex UriPatternAllowLastEmpty => mUriPatternAllowLastEmpty;
    }
}