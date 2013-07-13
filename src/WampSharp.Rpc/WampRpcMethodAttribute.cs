using System;

namespace WampSharp.Rpc
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class WampRpcMethodAttribute : Attribute
    {
        private readonly string mProcUri;

        public WampRpcMethodAttribute(string procUri)
        {
            IsRelative = true;
            mProcUri = procUri;
        }

        public string ProcUri
        {
            get
            {
                return mProcUri;
            }
        }

        public bool IsRelative
        {
            get; 
            set;
        }
    }
}