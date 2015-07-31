#if PCL
using System;

namespace WampSharp.V2.CalleeProxy
{
    public class GeneratedCodeException : Exception
    {
        private readonly string mGeneratedCode;

        public GeneratedCodeException(string generatedCode)
            : base("Try the code attached in the GeneratedCode property.")
        {
            mGeneratedCode = generatedCode;
        }

        public string GeneratedCode
        {
            get { return mGeneratedCode; }
        }
    }
}
#endif