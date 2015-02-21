using System;

namespace WampSharp.V2.Rpc
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class WampProcedureAttribute : Attribute
    {
        private readonly string mProcedure;

        public WampProcedureAttribute(string procedure)
        {
            this.mProcedure = procedure;
        }

        public string Procedure
        {
            get
            {
                return mProcedure;
            }
        }
    }
}