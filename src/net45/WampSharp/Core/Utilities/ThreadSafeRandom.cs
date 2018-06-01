using System;
using System.Threading;

namespace WampSharp.Core.Utilities
{
    internal class ThreadSafeRandom
    {
        private readonly Random mSeedGenerator = new Random();

        private readonly object mLock = new object();

        private readonly ThreadLocal<Random> mRandom;

        public ThreadSafeRandom()
        {
            mRandom = new ThreadLocal<Random>(CreateGenerator);
        }

        private Random CreateGenerator()
        {
            int seed;

            lock (mLock)
            {
                seed = mSeedGenerator.Next();
            }

            Random result = new Random(seed);

            return result;
        }

        public Random Random => mRandom.Value;

        public int Next()
        {
            return Random.Next();
        }

        public int Next(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

        public int Next(int maxValue)
        {
            return Random.Next(maxValue);
        }

        public double NextDouble()
        {
            return Random.NextDouble();
        }

        public void NextBytes(byte[] buffer)
        {
            Random.NextBytes(buffer);
        }
    }
}