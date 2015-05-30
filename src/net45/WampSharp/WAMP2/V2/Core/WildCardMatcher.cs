using System;
using System.Linq;

namespace WampSharp.V2.Core
{
    internal class WildCardMatcher
    {
        private readonly string mWildcard;
        private readonly int[] mNonEmptyPartIndexes;
        private readonly string[] mParts;

        public WildCardMatcher(string wildcard)
        {
            mWildcard = wildcard;

            mParts = wildcard.Split('.');

            mNonEmptyPartIndexes =
                mParts
                    .Select((part, index) => new {part, index})
                    .Where(x => x.part != String.Empty)
                    .Select(x => x.index).ToArray();
        }

        public bool IsMatched(string[] uriParts)
        {
            if (mParts.Length != uriParts.Length)
            {
                return false;
            }

            if (mNonEmptyPartIndexes.All(index => uriParts[index] == mParts[index]))
            {
                return true;
            }

            return false;
        }
    }
}