using System;

namespace WampSharp.Rpc.Server
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class WampRpcMethodAttribute : Attribute
    {
        private readonly string mProcUri;

        public WampRpcMethodAttribute(string procUri)
        {
            mProcUri = procUri;
        }

        public string ProcUri
        {
            get { return mProcUri; }
        }
    }
}