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

        public string ResolveCurie(string curie)
        {
            string result;

            if (mPrefixToUri.TryGetValue(curie, out result))
            {
                return result;
            }

            return null;
        }

        public void Map(string prefix, string uri)
        {
            mPrefixToUri[prefix] = uri;
        }
    }
}