using System;
using System.Linq;
using WampSharp.Core.Utilities;

namespace WampSharp.V1.Core.Listener
{
    /// <summary>
    /// An implementation of <see cref="IWampSessionIdGenerator"/>.
    /// </summary>
    public class WampSessionIdGenerator : IWampSessionIdGenerator
    {
        #region Members

        private const int ID_LENGTH = 16;
        private readonly char[] mCharacters;
        private readonly ThreadSafeRandom mRandom;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WampSessionIdGenerator"/>.
        /// </summary>
        public WampSessionIdGenerator()
        {
            mCharacters =
                Enumerable.Range('0', 10)
                          .Concat(Enumerable.Range('A', 'Z' - 'A' + 1))
                          .Concat(Enumerable.Range('a', 'z' - 'a' + 1))
                          .Select(x => (char) x).ToArray();

            mRandom = new ThreadSafeRandom();
        }

        #endregion

        #region Public Methods

        public string Generate()
        {
            char[] resultArray = new char[ID_LENGTH];

            for (int i = 0; i < ID_LENGTH; i++)
            {
                resultArray[i] = mCharacters[Random.Next(mCharacters.Length)];
            }

            return new string(resultArray);
        }

        private Random Random => mRandom.Random;

        #endregion
    }
}