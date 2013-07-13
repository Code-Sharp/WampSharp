using System;
using System.Collections.Generic;

namespace WampSharp.Core.Curie
{
    /// <summary>
    /// An implementation of <see cref="IWampCurieMapper"/>.
    /// </summary>
    public class WampCurieMapper : IWampCurieMapper
    {
        private readonly IDictionary<string, string> mPrefixToUri = 
            new Dictionary<string, string>();

        public void Map(string prefix, string uri)
        {
            mPrefixToUri[prefix] = uri;
        }

        public string Resolve(string curie)
        {
            Uri uri;

            if (Uri.TryCreate(curie, UriKind.Absolute, out uri))
            {
                return curie;
            }

            int index = curie.IndexOf(":", StringComparison.Ordinal);

            if (index > 0)
            {
                string prefix = curie.Substring(0, index);

                string mappedUri;

                if (mPrefixToUri.TryGetValue(prefix, out mappedUri))
                {
                    string rest = curie.Substring(index + 1);
                    string result = mappedUri + rest;
                    return result;
                }
            }

            return curie;
        }
    }
}