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

            if (extra == null || !extra.TryGetValue("salt", out string salt))
            {
                return null;
            }
            else
            {
                CraChallenge result = new CraChallenge();

                result.Salt = salt;


                if (extra.TryGetValue("iterations", out string strTemp))
                {
                    if (int.TryParse(strTemp, out int iterations))
                    {
                        result.Iterations = iterations;
                    }
                }


                if (extra.TryGetValue("keylen", out strTemp))
                {
                    if (int.TryParse(strTemp, out int keyLen))
                    {
                        result.KeyLength = keyLen;
                    }
                }

                return result;
            }
        }
    }
}