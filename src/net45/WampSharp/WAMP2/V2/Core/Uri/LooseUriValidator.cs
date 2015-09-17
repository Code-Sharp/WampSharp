using System.Text.RegularExpressions;

namespace WampSharp.V2.Core
{
    public class LooseUriValidator : WampUriValidator
    {
        /// <summary>
        /// Loose URI check allowing empty URI components
        /// </summary>
        private readonly Regex mUriPatternAllowEmpty = new Regex(@"^(([^\s\.#]+\.)|\.)*([^\s\.#]+)?$", RegexOptions.Compiled);

        /// <summary>
        /// Loose URI check disallowing empty URI components
        /// </summary>
        private readonly Regex mUriPatternDisallowEmpty = new Regex(@"^([^\s\.#]+\.)*([^\s\.#]+)$", RegexOptions.Compiled);

        /// <summary>
        /// Loose URI check disallowing empty URI components in all but the last component
        /// </summary>
        private readonly Regex mUriPatternAllowLastEmpty = new Regex(@"^([^\s\.#]+\.)*([^\s\.#]*)$", RegexOptions.Compiled);

        public override Regex UriPatternAllowEmpty
        {
            get
            {
                return mUriPatternAllowEmpty;
            }
        }

        public override Regex UriPatternDisallowEmpty
        {
            get
            {
                return mUriPatternDisallowEmpty;
            }
        }

        public override Regex UriPatternAllowLastEmpty
        {
            get
            {
                return mUriPatternAllowLastEmpty;
            }
        }
    }
}