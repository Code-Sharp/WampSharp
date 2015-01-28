using System.Runtime.Remoting.Messaging;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    /// <summary>
    /// Includes information about the current invocation.
    /// </summary>
    public class WampInvocationContext
    {
        #region Static Members

        public static WampInvocationContext Current
        {
            get
            {
                return (WampInvocationContext)CallContext.LogicalGetData(typeof(WampInvocationContext).Name);
            }
            internal set
            {
                CallContext.LogicalSetData(typeof(WampInvocationContext).Name, value);
            }
        }

        #endregion

        #region Members

        private readonly InvocationDetails mInvocationDetails;

        #endregion

        #region Constructor

        internal WampInvocationContext(InvocationDetails invocationDetails)
        {
            mInvocationDetails = invocationDetails;
        }

        #endregion

        #region Properties

        public InvocationDetails InvocationDetails
        {
            get
            {
                return mInvocationDetails;
            }
        }

        #endregion
    }
}