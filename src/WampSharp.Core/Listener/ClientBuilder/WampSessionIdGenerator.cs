using System;
using System.Linq;

namespace WampSharp.Core.Listener
{
    public class WampSessionIdGenerator : IWampSessionIdGenerator
    {
        private const int ID_LENGTH = 16;
        private readonly char[] mCharacters;
        private readonly Random mRandom;

        public WampSessionIdGenerator()
        {
            mCharacters =
                Enumerable.Range('0', 10)
                          .Concat(Enumerable.Range('A', 'Z' - 'A' + 1))
                          .Concat(Enumerable.Range('a', 'z' - 'a' + 1))
                          .Select(x => (char) x).ToArray();

            mRandom = new Random();
        }

        public string Generate()
        {
            char[] resultArray = new char[ID_LENGTH];

            for (int i = 0; i < ID_LENGTH; i++)
            {
                resultArray[i] = mCharacters[mRandom.Next(mCharacters.Length)];
            }

            return new string(resultArray);
        }
    }
}