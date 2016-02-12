using System;

namespace WampSharp.V2.Rpc
{
    public class WampMemberAttributeBase : Attribute
    {
        private string mMemberName;

        /// <summary>
        /// Initializes a new instance of <see cref="WampProcedureAttribute"/> given
        /// the procedure uri this method is mapped to.
        /// </summary>
        /// <param name="procedure">The given the procedure uri this method is mapped to.</param>
        public WampMemberAttributeBase(string procedure)
        {
            this.mMemberName = procedure;
        }

        /// <summary>
        /// Gets the procedure uri this method is mapped to.
        /// </summary>
        public string Procedure
        {
            get
            {
                return mMemberName;
            }
        }
    }

    /// <summary>
    /// Indicates this method respresents a WAMPv2 rpc procedure.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class WampProcedureAttribute : WampMemberAttributeBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="WampProcedureAttribute"/> given
        /// the procedure uri this method is mapped to.
        /// </summary>
        /// <param name="procedure">The given the procedure uri this method is mapped to.</param>
        public WampProcedureAttribute(string procedure) : base(procedure)
        {
        }
    }

    /// <summary>
    /// Indicates this method respresents a WAMPv2 rpc member (that has rpc members/procedures).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class WampMemberAttribute : WampMemberAttributeBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="WampProcedureAttribute"/> given
        /// the procedure uri this method is mapped to.
        /// </summary>
        /// <param name="procedure">The given the procedure uri this method is mapped to.</param>
        public WampMemberAttribute(string procedure) : base(procedure)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="WampProcedureAttribute"/> given
        /// the procedure uri this method is mapped to.
        /// </summary>
        /// <param name="procedure">The given the procedure uri this method is mapped to.</param>
        public WampMemberAttribute() : base(String.Empty)
        {
        }
    }
}