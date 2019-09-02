using System;

namespace WampSharp.V2.CalleeProxy
{
    public class GeneratedCodeException : Exception
    {
        public GeneratedCodeException(string generatedCode)
            : base("Try the code attached in the GeneratedCode property.")
        {
            GeneratedCode = generatedCode;
        }

        public string GeneratedCode { get; }
    }
}