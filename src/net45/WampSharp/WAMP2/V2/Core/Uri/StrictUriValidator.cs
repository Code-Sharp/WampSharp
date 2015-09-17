using System.Text.RegularExpressions;

namespace WampSharp.V2.Core
{
    public class StrictUriValidator : WampUriValidator
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

        public StrictUriValidator()
        {
#if !PCL
            RegexOptions regexOptions = RegexOptions.Compiled;
#else
            RegexOptions regexOptions = RegexOptions.None;
#endif

            mUriPatternAllowEmpty = new Regex(@"^(([0-9a-z_]+\.)|\.)*([0-9a-z_]+)?$", regexOptions);
            mUriPatternDisallowEmpty = new Regex(@"^([0-9a-z_]+\.)*([0-9a-z_]+)$", regexOptions);
            mUriPatternAllowLastEmpty = new Regex(@"^([0-9a-z_]+\.)*([0-9a-z_]*)$", regexOptions);
        }

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