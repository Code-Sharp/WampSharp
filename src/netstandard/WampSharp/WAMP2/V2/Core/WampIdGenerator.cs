using System;
using WampSharp.Core.Utilities;

namespace WampSharp.V2.Core
{
    /// <summary>
    /// A default implementation of <see cref="IWampIdGenerator"/>.
    /// </summary>
    public class WampIdGenerator : IWampIdGenerator
    {
        private readonly long mLimit = (long)Math.Pow(2, 50);
        private readonly ThreadSafeRandom mRandom = new ThreadSafeRandom();

        public long Generate()
        {
            Random random = mRandom.Random;
            return NextLong(random, 0, mLimit);
        }

        #region From StackOverflow

        //returns a uniformly random ulong between ulong.min inclusive and ulong.max inclusive
        private static ulong NextULong(Random Rng)
        {
            byte[] buf = new byte[8];
            Rng.NextBytes(buf);
            return BitConverter.ToUInt64(buf, 0);
        }

        //returns a uniformly random ulong between ulong.min and max without modulo bias
        private static ulong NextULong(Random Rng, ulong Max, bool inclusiveUpperBound = false)
        {
            return NextULong(Rng, ulong.MinValue, Max, inclusiveUpperBound);
        }

        //returns a uniformly random ulong between min and max without modulo bias
        private static ulong NextULong(Random Rng, ulong Min, ulong Max, bool inclusiveUpperBound = false)
        {
            ulong range = Max - Min;

            if (inclusiveUpperBound)
            {
                if (range == ulong.MaxValue)
                {
                    return NextULong(Rng);
                }

                range++;
            }

            if (range <= 0)
            {
                throw new ArgumentOutOfRangeException("max must be greater than min when inclusiveUpperBound is false, and greater than or equal to when true", "max");
            }

            ulong limit = ulong.MaxValue - ulong.MaxValue % range;
            ulong r;
            do
            {
                r = NextULong(Rng);
            } while (r > limit);

            return r % range + Min;
        }

        //returns a uniformly random long between long.min inclusive and long.max inclusive
        private static long NextLong(Random Rng)
        {
            byte[] buf = new byte[8];
            Rng.NextBytes(buf);
            return BitConverter.ToInt64(buf, 0);
        }

        //returns a uniformly random long between long.min and max without modulo bias
        private static long NextLong(Random Rng, long Max, bool inclusiveUpperBound = false)
        {
            return NextLong(Rng, long.MinValue, Max, inclusiveUpperBound);
        }

        //returns a uniformly random long between min and max without modulo bias
        private static long NextLong(Random Rng, long min, long max, bool inclusiveUpperBound = false)
        {
            ulong range = (ulong)(max - min);

            if (inclusiveUpperBound)
            {
                if (range == ulong.MaxValue)
                {
                    return NextLong(Rng);
                }

                range++;
            }

            if (range <= 0)
            {
                throw new ArgumentOutOfRangeException("max must be greater than min when inclusiveUpperBound is false, and greater than or equal to when true", "max");
            }

            ulong limit = ulong.MaxValue - ulong.MaxValue % range;
            ulong random;
            do
            {
                random = NextULong(Rng);
            } 
            while (random > limit);

            return (long)(random % range + (ulong)min);
        }

        #endregion
    }
}