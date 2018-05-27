using System;

namespace WampSharp.V2.Rpc
{
    /// <summary>
    /// Indicates this method respresents a WAMPv2 rpc procedure.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class WampProcedureAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of <see cref="WampProcedureAttribute"/> given
        /// the procedure uri this method is mapped to.
        /// </summary>
        /// <param name="procedure">The given the procedure uri this method is mapped to.</param>
        public WampProcedureAttribute(string procedure)
        {
            this.Procedure = procedure;
        }

        /// <summary>
        /// Gets the procedure uri this method is mapped to.
        /// </summary>
        public string Procedure { get; }
    }
}