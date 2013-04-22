using System.Collections.Generic;

namespace WampSharp.Core.Curie
{
    public class WampCurieMapper : IWampCurieMapper
    {
        private readonly IDictionary<string, string> mCurieToUri = 
            new Dictionary<string, string>();

        public string ResolveCurie(string curie)
        {
            string result;

            if (mCurieToUri.TryGetValue(curie, out result))
            {
                return result;
            }

            return null;
        }

        public void Map(string curie, string uri)
        {
            mCurieToUri[curie] = uri;
        }
    }
}