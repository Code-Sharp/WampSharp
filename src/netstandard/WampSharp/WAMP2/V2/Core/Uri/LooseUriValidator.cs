using System.Text.RegularExpressions;

namespace WampSharp.V2.Core
{
    /// <summary>
    /// A <see cref="IWampUriValidator"/> that validates uris 
    /// via the loose uri definition.
    /// </summary>
    public class LooseUriValidator : WampUriRegexValidator
    {
        /// <summary>
        /// Loose URI check allowing empty URI components
        /// </summary>
        private readonly Regex mUriPatternAllowEmpty;

        /// <summary>
        /// Loose URI check disallowing empty URI components
        /// </summary>
        private readonly Regex mUriPatternDisallowEmpty;

        /// <summary>
        /// Loose URI check disallowing empty URI components in all but the last component
        /// </summary>
        private readonly Regex mUriPatternAllowLastEmpty;

        /// <summary>
        /// Instantiates a new instance of the <see cref="LooseUriValidator"/> class.
        /// </summary>
        public LooseUriValidator()
        {
            RegexOptions regexOptions = RegexOptions.Compiled;

            mUriPatternAllowEmpty = new Regex(@"^(([^\s\.#]+\.)|\.)*([^\s\.#]+)?$", regexOptions);
            mUriPatternDisallowEmpty = new Regex(@"^([^\s\.#]+\.)*([^\s\.#]+)$", regexOptions);
            mUriPatternAllowLastEmpty = new Regex(@"^([^\s\.#]+\.)*([^\s\.#]*)$", regexOptions);
        }

        public override Regex UriPatternAllowEmpty => mUriPatternAllowEmpty;

        public override Regex UriPatternDisallowEmpty => mUriPatternDisallowEmpty;

        public override Regex UriPatternAllowLastEmpty => mUriPatternAllowLastEmpty;
    }
}