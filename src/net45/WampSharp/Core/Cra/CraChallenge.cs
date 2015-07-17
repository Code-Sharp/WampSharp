using System.Collections.Generic;

namespace WampSharp.Core.Cra
{
    internal class CraChallenge : IWampCraChallenge
    {
        private CraChallenge()
        {
        }

        public string Salt { get; private set; }
        public int? Iterations { get; private set; }
        public int? KeyLength { get; private set; }

        public static CraChallenge Create(IDictionary<string, string> extra)
        {
            string salt;

            if (extra == null || !extra.TryGetValue("salt", out salt))
            {
                return null;
            }
            else
            {
                CraChallenge result = new CraChallenge();

                result.Salt = salt;

                string strTemp;

                int iterations;

                if (extra.TryGetValue("iterations", out strTemp))
                {
                    if (int.TryParse(strTemp, out iterations))
                    {
                        result.Iterations = iterations;
                    }
                }

                int keyLen;

                if (extra.TryGetValue("keylen", out strTemp))
                {
                    if (int.TryParse(strTemp, out keyLen))
                    {
                        result.KeyLength = keyLen;
                    }
                }

                return result;
            }
        }
    }
}