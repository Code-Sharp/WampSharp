using System;

namespace WampSharp.Rpc
{
    /// <summary>
    /// Indicates a method is a WAMP rpc service method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class WampRpcMethodAttribute : Attribute
    {
        private readonly string mProcUri;

        /// <summary>
        /// Intializes a new instance of <see cref="WampRpcMethodAttribute"/>.
        /// </summary>
        /// <param name="procUri"></param>
        public WampRpcMethodAttribute(string procUri)
        {
            IsRelative = true;
            mProcUri = procUri;
        }

        /// <summary>
        /// The proc uri of this method.
        /// </summary>
        public string ProcUri
        {
            get
            {
                return mProcUri;
            }
        }

        /// <summary>
        /// Gets/sets a value indicating whether the proc uri is
        /// relative to a base uri or not.
        /// </summary>
        public bool IsRelative
        {
            get; 
            set;
        }
    }
}